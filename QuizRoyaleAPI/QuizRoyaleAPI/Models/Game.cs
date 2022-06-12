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
        private System.Timers.Timer _coolDownTimer;
        public int _minimumPlayers { get; } = 11;
        public int _maximumPlayers { get; } = 2000;
        public IDictionary<CategoryDTO, float> _categories { get; set; }
        private BoosterFactory _boosterFactory;
        private int _questionTimeInMili;
        private bool _inProgress;
        private const int WIN_XP = 500;
        private const int QUESTION_XP = 75;
        private const int WIN_COINS = 200;
        private const int QUESTION_COINS = 25;
        private int _miliStarted;

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
            if(State.CurrentGame != null)
            {
                _timer = new System.Timers.Timer(questionTime);
                // Hook up the Tick event for the timer. 
                _timer.Elapsed += this.NextQuestion;
                _timer.AutoReset = false;
                _timer.Enabled = true;
                _miliStarted = DateTime.Now.Millisecond;
            }
        }

        private void SetCooldownTimer() 
        {
            if (State.CurrentGame != null)
            {
                // Create a timer with a variable interval.
                _coolDownTimer = new System.Timers.Timer(10000); // 10 seconden
                                                                 // Hook up the Tick event for the timer. 
                _coolDownTimer.Elapsed += this.StartNextQuestion;
                _coolDownTimer.AutoReset = false;
                _coolDownTimer.Enabled = true;
            }
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
        private void NextQuestion(object? source, ElapsedEventArgs e)
        {
            if (this._currentQuestion != null)
            {
                this.SendResultsFromLastQuestion();
            }

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
                        _currentQuestion.Category = cat;
                        this.SendNewQuestion();
                        this.SetCooldownTimer();
                        break;
                    }
                }
            }
        }

        // Stuurt de volgende vraag
        public async Task SendNewQuestion()
        {
            await State.GetHubContext().Clients.All.SendAsync("newQuestion", _currentQuestion);// Documented
            Console.WriteLine("----------------> " + _currentQuestion.RightAnswer + " is het goede antwoord <-----------------");
        }

        // haalt de resultaten van de vorige vraag op om te sturen
        private void SendResultsFromLastQuestion()
        {
            if (this._allResponses.Count > 0)
            {
                IList<string> playersToEliminate = new List<string>();
                foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
                {
                    if (this._allResponses.ContainsKey(player.Value))
                    {
                        if (this._allResponses[player.Value] == this._currentQuestion.RightAnswer || this._allResponses[player.Value] == '*')
                        {
                            using (var scope = State.ServiceProvider.CreateScope())
                            {
                                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                                _PlayerService.GiveRewards(player.Value.Username, QUESTION_XP, QUESTION_COINS);
                            }
                            this.SendResultToPlayer(player.Key, true, QUESTION_XP, QUESTION_COINS);
                            Console.WriteLine(player.Value.Username + " heeft het goed!");
                        }
                        else
                        {
                            this.SendResultToPlayer(player.Key, false, 0, 0);
                            playersToEliminate.Add(player.Key);
                            Console.WriteLine(player.Value.Username + " heeft het fout!");
                            //this.EliminatePlayer(player.Key);
                        }
                    }
                    else
                    {
                        this.SendResultToPlayer(player.Key, false, 0, 0);
                        Console.WriteLine(player.Value.Username + " heeft niet geantwoord!");
                        playersToEliminate.Add(player.Key);
                        //this.EliminatePlayer(player.Key);
                    }
                }
                Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn voor de purge");
                foreach (string id in playersToEliminate)
                { 
                    this.EliminatePlayer(id);
                }
                Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn naa de purge");
            }
            else 
            {
                foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
                {
                    this.EliminatePlayer(player.Key);
                    Console.WriteLine("niemand heeft geantwoord");
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

            State.GetHubContext().Clients.All.SendAsync("playersLeft", this._allPlayers.Values); // Doucumented
        }

        // Als alle spelers af zijn is er geen winner en is de game voorbij
        public async Task EndTie()
        { 
            State.CurrentGame = null;
            RemoveTimers();
            Console.WriteLine("gelijkspelgelijkspelgelijkspelgelijkspelgelijkspelgelijkspel");
        }

        // Als er maar 1 speler over is dan wint deze speler
        public async Task EndWin(string winnerName)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                _PlayerService.GiveRewards(winnerName, WIN_XP, WIN_COINS);
                _PlayerService.GiveWin(winnerName);
            }

                await State.GetHubContext().Clients.All.SendAsync("Win", WIN_XP, WIN_COINS);// Documented
            State.CurrentGame = null;
            RemoveTimers();
            Console.WriteLine("winnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnn");
        }

        private void RemoveTimers()
        {
            _timer.Elapsed -= NextQuestion;
            _timer.Elapsed -= StartNextQuestion;
            _startDelayTimer.Elapsed -= startGame;
            _timer.Stop();
            _startDelayTimer.Stop();
            _timer.Dispose();
            _startDelayTimer.Dispose();
        }

        // Stuurt de resultaat van de vorige vraag 
        private async Task SendResultToPlayer(string conectionId, bool result, int xp, int coins)
        {
            await State.GetHubContext().Clients.Client(conectionId).SendAsync("result", result, xp, coins); // Documented
        }

        // Registreerd een antwoord
        public async Task AnswerQuestion(char id, string conectionId)
        {
            InGamePlayerDTO player = this._allPlayers[conectionId];
            if (!this._allResponses.ContainsKey(player))
            { 
                this._allResponses.Add(player, id);
                await State.GetHubContext().Clients.All.SendAsync("playerAwnsered", player, (DateTime.Now.Millisecond - _miliStarted) / 1000.0);
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
                this.SetTimer(10);
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