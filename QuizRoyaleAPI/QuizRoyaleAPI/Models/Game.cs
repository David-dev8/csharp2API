using System.Timers;
using QuizRoyaleAPI.Services.Data;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace QuizRoyaleAPI.Models
{
    public class Game
    {
        private QuestionDTO _currentQuestion;
        private IDictionary<string, InGamePlayerDTO> _allPlayers;
        private IDictionary<InGamePlayerDTO, char> _allResponses;
        public System.Timers.Timer _timer { get; set; }
        private System.Timers.Timer _startDelayTimer;
        public int _minimumPlayers { get; } = 4;
        public int _maximumPlayers { get; } = 6;
        private event EventHandler _questionEnded;
        private event EventHandler _playerWon;
        public IDictionary<CategoryDTO, float> _categories { get; set; }
        private BoosterFactory _boosterFactory;
        private int _questionTimeInMili;
        private bool _inProgress;

        public Game(int questionTimeInMili)
        {
            this._currentQuestion = null;
            this._allPlayers = new Dictionary<string, InGamePlayerDTO>();
            this._allResponses = new Dictionary<InGamePlayerDTO, char>();
            this._categories = new Dictionary<CategoryDTO, float>();
            this._boosterFactory = new BoosterFactory();
            this._questionTimeInMili = questionTimeInMili;
            this._inProgress = false;

            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _QuestionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                IEnumerable<CategoryDTO> categories = _QuestionService.GetCategories();
                foreach (CategoryDTO cat in categories)
                {
                    _categories.Add(cat, (float)100.0 / categories.Count());
                }
            }
        }

        // Set de timer voor de vraag
        public void SetTimer(int questionTime)
        {
            // Create a timer with a variable interval.
            _timer = new System.Timers.Timer(questionTime);
            // Hook up the Tick event for the timer. 
            _timer.Elapsed += this.NextQuestion;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void SetCooldownTimer() 
        {
            // Create a timer with a variable interval.
            _timer = new System.Timers.Timer(10000); // 10 seconden
            // Hook up the Tick event for the timer. 
            _timer.Elapsed += this.StartNextQuestion;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        //start de timer voor de volgende vraag weer op
        private void StartNextQuestion(Object source, ElapsedEventArgs e)
        { 
            this.SetTimer(this._questionTimeInMili);
            this.NotifyNextQuestion();
        }

        //Laat iedereen weten dat de volgende vraag begint
        private async Task NotifyNextQuestion() 
        {
            await State.GetHubContext().Clients.All.SendAsync("StartQuestion");// Documented
        }

        // Haalt en stuurt de volgende vraag op
        private void NextQuestion(Object source, ElapsedEventArgs e)
        {

            this.SendResultsFromLastQuestion();

            Random rnd = new Random();
            int randomInt = rnd.Next(100);
            float counter = 0;

            foreach (CategoryDTO cat in this._categories.Keys)
            {
                this._allResponses = new Dictionary<InGamePlayerDTO, char>();
                counter += this._categories[cat];

                if (randomInt <= counter)
                {
                    using (var scope = State.ServiceProvider.CreateScope())
                    {
                        var _QuestionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                        this._currentQuestion = _QuestionService.GetQuestionByCategoryId(cat.Id); // Get random element from array
                        this.SendNewQuestion(cat.Name);
                        this.SetCooldownTimer();
                        break;
                    }
                }
            }
        }

        // Stuurt de volgende vraag
        public async Task SendNewQuestion(string catName)
        {
            await State.GetHubContext().Clients.All.SendAsync("newQuestion", this._currentQuestion);// Documented
        }

        // haalt de resultaten van de vorige vraag op om te sturen
        private void SendResultsFromLastQuestion()
        {
            if (this._allResponses.Count > 0)
            {
                foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
                {
                    if (this._allResponses.ContainsKey(player.Value))
                    {
                        if (this._allResponses[player.Value] == this._currentQuestion.RightAnswer || this._allResponses[player.Value] == '*')
                        {
                            this.SendResultToPlayer(player.Key, true);
                        }
                        else
                        {
                            this.SendResultToPlayer(player.Key, false);
                            Console.WriteLine("er gaat iemand dood");
                            this.EliminatePlayer(player.Key);
                        }
                    }
                    else
                    {
                        this.SendResultToPlayer(player.Key, false);
                        this.EliminatePlayer(player.Key);
                    }
                }
            }
            else 
            {
                foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
                {
                    this.EliminatePlayer(player.Key);
                }
            }

            if (this._allPlayers.Count == 0)
            {
                this.EndTie();
            }
            else if (this._allPlayers.Count == 1) 
            {
                this.EndWin(this._allPlayers.First().Value.Username);
            }
        }

        // Als alle spelers af zijn is er geen winner en is de game voorbij
        public async Task EndTie()
        { 
            State.CurrentGame = null;
        }

        // Als er maar 1 speler over is dan wint deze speler
        public async Task EndWin(string winnerName)
        {
            await State.GetHubContext().Clients.All.SendAsync("Win", winnerName);// Documented
            State.CurrentGame = null;
        }

        // Stuurt de resultaat van de vorige vraag 
        private async Task SendResultToPlayer(string conectionId, bool result)
        {
            await State.GetHubContext().Clients.Client(conectionId).SendAsync("result", result); // Documented
        }

        // Registreerd een antwoord
        public async Task AnswerQuestion(char id, string conectionId)
        {
            InGamePlayerDTO player = this._allPlayers[conectionId];
            if (!this._allResponses.ContainsKey(player))
            { 
                this._allResponses.Add(player, id);
                await State.GetHubContext().Clients.All.SendAsync("playerAwnsered", player);
            }
        }

        // Gebruikt een boost
        public void UseBoost(string type, string options)
        {
            _boosterFactory.getBooster(type).use(this, options);
        }

        // Elimineerd een speler
        public async Task EliminatePlayer(string playerID)
        {
            this._allPlayers.Remove(playerID);
            await State.GetHubContext().Clients.Client(playerID).SendAsync("gameOver"); // Doucumented
        }

        // Kijkt of het mogelijk is om een potje te starten
        public bool CanStart()
        {
            if (this._allPlayers.Count >= this._minimumPlayers && this._allPlayers.Count <= this._maximumPlayers && this._inProgress == false)
            {
                return true;
            }
            return false;
        }

        // Start een potje
        public async Task Start()
        {
            if (this.CanStart())
            {
                await State.GetHubContext().Clients.All.SendAsync("start"); // Documented
                this.SetTimer(this._questionTimeInMili);
                this._inProgress = true;
            }
        }

        // Kijkt om het mogelijk is voor een speler om te joinen
        public bool CanJoin()
        {
            if (this._allPlayers.Count < this._maximumPlayers && this._inProgress == false)
            {
                return true;
            }
            return false;
        }

        // Laat een speler Joinen
        public void Join(string name, string conectionId)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                InGamePlayerDTO player = _PlayerService.GetPlayerInGame(name);

                if (this.CanJoin())
                {
                    this._allPlayers.Add(conectionId, player);

                    if (this.CanStart())
                    {
                        this.SetStartTimer();
                    }
                }
            }
        }

        // Haalt de hoeveelheid spelers op
        public int GetAmountOfPlayers()
        {
            return this._allPlayers.Count();
        }

        // Set de timer voor het starten van een spel
        private void SetStartTimer()
        {
            if (_startDelayTimer != null)
            {
                // Reset de timer als en nieuwe deelnemer binnekomt
                _startDelayTimer.Stop();
                _startDelayTimer.Start();
            }
            else
            {
                // Create a timer with a variable interval.
                _startDelayTimer = new System.Timers.Timer(5000);
                // Hook up the Tick event for the timer. 
                _startDelayTimer.Elapsed += this.startGame;
                _startDelayTimer.AutoReset = false;
                _startDelayTimer.Enabled = true;
            }
        }

        // Methode om het startcomando uit te voeren
        private void startGame(Object source, ElapsedEventArgs e)
        {
            this.Start();

        }

        public IList<InGamePlayerDTO> GetPlayerNames()
        {
            IList<InGamePlayerDTO> names = new List<InGamePlayerDTO>();

            foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
            {
                names.Add(player.Value);
            }

            return names;
        }

        public InGamePlayerDTO GetPlayerObj(string name)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                InGamePlayerDTO player = _PlayerService.GetPlayerInGame(name);
                return player;
            }
        }

        public IList<MasteryDTO> getCategories()
        {
            IList<MasteryDTO> list = new List<MasteryDTO>();
            foreach (KeyValuePair<CategoryDTO, float> cat in this._categories)
            {
                list.Add(new MasteryDTO(cat.Key, cat.Value));
            }
            return list;
        }
    }
}