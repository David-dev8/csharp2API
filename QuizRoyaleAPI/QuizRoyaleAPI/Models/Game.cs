using System.Timers;
using QuizRoyaleAPI.Services.Data;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is de game, alle logika en data met betrekking tot een game wordt hiet afgehandeld
    /// </summary>
    public class Game
    {
        private IDictionary<string, InGamePlayerDTO> _allPlayers;
        private IDictionary<InGamePlayerDTO, char> _allResponses;
        private System.Timers.Timer _startDelayTimer;
        private System.Timers.Timer _coolDownTimer;
        private BoosterFactory _boosterFactory;
        private int _questionTimeInMili;
        private bool _inProgress;
        private const int WIN_XP = 500;
        private const int QUESTION_XP = 75;
        private const int WIN_COINS = 200;
        private const int QUESTION_COINS = 25;
        private long _miliStarted;

        public QuestionDTO CurrentQuestion { get; set; }
        public IDictionary<CategoryDTO, float> _categories { get; set; }
        public int MinimumPlayers { get; } = 11;
        public int MaximumPlayers { get; } = 2000;
        public System.Timers.Timer Timer { get; set; }

        public Game(int questionTimeInMili)
        {
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

        /// <summary>
        /// Start de timer voor de antwoord tijd van een vraag
        /// </summary>
        /// <param name="questionTime">Hoe lang de spelers hebben om te antwoorden in miliseconden</param>
        // Set de timer voor de vraag
        public void SetTimer(int questionTime)
        {
            // Create a timer with a variable interval.
            if(State.CurrentGame != null)
            {
                Timer = new System.Timers.Timer(questionTime);
                // Hook up the Tick event for the timer. 
                Timer.Elapsed += this.NextQuestion;
                Timer.AutoReset = false;
                Timer.Enabled = true;
                _miliStarted = GetCurrentMilis();
            }
        }

        /// <summary>
        /// Start een timer voor de cooldown tussen vragen in
        /// </summary>
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

        /// <summary>
        /// Start de volgende vraag
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">eventArgs</param>
        private void StartNextQuestion(Object source, ElapsedEventArgs e)
        { 
            this.SetTimer(this._questionTimeInMili);
            this.NotifyNextQuestion();
        }

        /// <summary>
        /// Laat iedereen weten dat de volgende vraag begint
        /// </summary>
        /// <returns>void</returns>
        private async Task NotifyNextQuestion() 
        {
            await State.GetHubContext().Clients.All.SendAsync("StartQuestion");// Documented
        }

        /// <summary>
        /// kiest en stuurt de volgende vraag op. 
        /// Vragen worden gekozen op basis van een random categorie. 
        /// De kansen van deze random categorieen kunnen worden beinvloed met boosters. 
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">EventArgs</param>
        private void NextQuestion(object? source, ElapsedEventArgs e)
        {
            if (this.CurrentQuestion != null)
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
                        this.CurrentQuestion = _QuestionService.GetQuestionByCategoryId(cat.Id);
                        CurrentQuestion.Category = cat;
                        this.SendNewQuestion();
                        this.SetCooldownTimer();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Stuurt de inhoud van de volgende vraag op
        /// </summary>
        /// <returns>stuurt de QuestionDTO op naar alle clients</returns>
        public async Task SendNewQuestion()
        {
            await State.GetHubContext().Clients.All.SendAsync("newQuestion", CurrentQuestion);// Documented
            Console.WriteLine("----------------> " + CurrentQuestion.RightAnswer + " is het goede antwoord <-----------------");
        }

        /// <summary>
        /// haalt de resultaten van de vorige vraag op om te sturen
        /// </summary>
        private void SendResultsFromLastQuestion()
        {
            ICollection<string> playersToEliminate = new List<string>();
            if (this._allResponses.Count > 0)
            {
                using (var scope = State.ServiceProvider.CreateScope())
                {
                    var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();

                    foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
                    {
                        if (_allResponses.ContainsKey(player.Value)
                            && (_allResponses[player.Value] == this.CurrentQuestion.RightAnswer || this._allResponses[player.Value] == '*'))
                        {
                            _PlayerService.RegisterAnswer(player.Value.Username, true, CurrentQuestion.Id);
                            _PlayerService.GiveRewards(player.Value.Username, QUESTION_XP, QUESTION_COINS);
                            this.SendResultToPlayer(player.Key, true, QUESTION_XP, QUESTION_COINS);
                            Console.WriteLine(player.Value.Username + " heeft het goed!");
                        }
                        else
                        {
                            _PlayerService.RegisterAnswer(player.Value.Username, false, CurrentQuestion.Id);
                            this.SendResultToPlayer(player.Key, false, 0, 0);
                            Console.WriteLine(player.Value.Username + " heeft geen goed antwoord gegegeven!");
                            playersToEliminate.Add(player.Key);
                        }
                    }
                }
                Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn voor de purge");
            }
            else
            {
                playersToEliminate = _allPlayers.Keys;
            }

            int finalPosition = _allPlayers.Count;
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                foreach (string id in playersToEliminate)
                {
                    _PlayerService.RegisterResult(_allPlayers[id].Username, Mode.QUIZ_ROYALE, finalPosition);
                    this.EliminatePlayer(id);
                }
            }
            Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn naa de purge");

            State.GetHubContext().Clients.All.SendAsync("playersLeft", this._allPlayers.Values); // Doucumented

            if (this._allPlayers.Count == 0)
            {
                this.EndTie();
            }
            else if (this._allPlayers.Count == 1) 
            {
                this.EndWin(this._allPlayers.First().Value.Username);
            }
        }

        /// <summary>
        /// Eindigd het spel in gelijkspel. 
        /// Dit kan voorkomen als alle spelers een vraag fout of niet beantwoorden
        /// </summary>
        /// <returns>void</returns>
        // Als alle spelers af zijn is er geen winner en is de game voorbij
        public async Task EndTie()
        { 
            State.CurrentGame = null;
            RemoveTimers();
            Console.WriteLine("gelijkspelgelijkspelgelijkspelgelijkspelgelijkspelgelijkspel");
        }

        /// <summary>
        /// Eindigd het spel in een winst voor een speler
        /// </summary>
        /// <param name="winnerName">De username van de speler die heeft gewonnen</param>
        /// <returns>void</returns>
        // Als er maar 1 speler over is dan wint deze speler
        public async Task EndWin(string winnerName)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                _PlayerService.GiveRewards(winnerName, WIN_XP, WIN_COINS);
                _PlayerService.GiveWin(winnerName);
                _PlayerService.RegisterResult(winnerName, Mode.QUIZ_ROYALE, 1);
            }

            await State.GetHubContext().Clients.All.SendAsync("Win", WIN_XP, WIN_COINS);// Documented
            State.CurrentGame = null;
            RemoveTimers();
            Console.WriteLine("winnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnnwinnn");
        }

        /// <summary>
        /// Collect alle timers om het spel netjes af te sluiten
        /// </summary>
        private void RemoveTimers()
        {
            Timer.Elapsed -= NextQuestion;
            Timer.Elapsed -= StartNextQuestion;
            _startDelayTimer.Elapsed -= startGame;
            Timer.Stop();
            _startDelayTimer.Stop();
            Timer.Dispose();
            _startDelayTimer.Dispose();
        }

        /// <summary>
        /// Stuurt de resultaten van de vorige vraag op
        /// </summary>
        /// <param name="conectionId">De conectie waarnaa het resultaat moet worden gestuurd</param>
        /// <param name="result">Het resultaat dat de speler heeft</param>
        /// <param name="xp">Het aantal XP dat de speler heeft verdient met dit resultaat</param>
        /// <param name="coins">Het aantal coins dat de speler heeft verdient met dit resultaat</param>
        /// <returns>Geeft de client een event met zijn resultaten</returns>
        private async Task SendResultToPlayer(string conectionId, bool result, int xp, int coins)
        {
            await State.GetHubContext().Clients.Client(conectionId).SendAsync("result", result, xp, coins); // Documented
        }

        /// <summary>
        /// Registreerd een antwoord van een speler
        /// </summary>
        /// <param name="id">De answerID van de awnser die de speler heeft gegeven</param>
        /// <param name="conectionId">De conectie van de speler die het resultaat stuurt</param>
        /// <returns>Stuurt alle clients een event dat iemand heeft geantwoord met de tijd erbij</returns>
        public async Task AnswerQuestion(char id, string conectionId)
        {
            InGamePlayerDTO player = this._allPlayers[conectionId];
            if (!this._allResponses.ContainsKey(player))
            { 
                this._allResponses.Add(player, id);
                await State.GetHubContext().Clients.All.SendAsync("playerAnswered", player, (GetCurrentMilis() - _miliStarted) / 1000.0);
            }
        }

        // Haalt de huidige timestamp in milliseconden op
        private long GetCurrentMilis()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Gebruikt een boost
        /// </summary>
        /// <param name="type">De type Booster die je wilt gebruiken</param>
        /// <param name="options">De opties voor de booster</param>
        /// <param name="conncectionId">De connectieID van de speler</param>
        public void UseBoost(string type, string options, string conncectionId)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                _PlayerService.removeItem(this._allPlayers[conncectionId].Username, type);
            }
            _boosterFactory.getBooster(type).use(this, options);
            Console.WriteLine("De kansen zijn Nu: ");
            float totaal = 0;
            foreach (KeyValuePair<CategoryDTO, float> chance in this._categories)
            {
                Console.WriteLine(chance.Key.Name + " Heeft nu een kans van " + chance.Value + "%");
                totaal += chance.Value;
                Console.WriteLine("Totaal is nu " + totaal + "%");
            }
        }

        /// <summary>
        /// Elimineerd een speler uit een game
        /// </summary>
        /// <param name="playerID">De Conectie van de speler die moet worden geelimineerd</param>
        /// <returns>Geeft de geelimineerde speler een event om te laten weten dat hij af is</returns>
        public async Task EliminatePlayer(string playerID)
        {
            this._allPlayers.Remove(playerID);
            await State.GetHubContext().Clients.Client(playerID).SendAsync("gameOver"); // Doucumented
        }

        /// <summary>
        /// Kijkt of het mogelijk is om een spel te starten
        /// </summary>
        /// <returns>True als een spel gestart kan worden, anders false</returns>
        public bool CanStart()
        {
            if (this._allPlayers.Count >= this.MinimumPlayers && this._allPlayers.Count <= this.MaximumPlayers && this._inProgress == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Start een game
        /// </summary>
        /// <returns>Stuurt alle clients een event dat het spel is gestart</returns>
        public async Task Start()
        {
            if (this.CanStart())
            {
                await State.GetHubContext().Clients.All.SendAsync("start"); // Documented
                this.SetTimer(10);
                this._inProgress = true;
            }
        }

        /// <summary>
        /// Kijkt of het mogelijk is voor en nieuwe speler om te joinen
        /// </summary>
        /// <returns>True als de speler kan joinen, anders false</returns>
        public bool CanJoin()
        {
            if (this._allPlayers.Count < this.MaximumPlayers && this._inProgress == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Laat een speler joinen
        /// </summary>
        /// <param name="name">De naam van de speler die joined</param>
        /// <param name="conectionId">De conectie van de speler die joined</param>
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

        /// <summary>
        /// Haalt de hoeveelheid spelers in de game op
        /// </summary>
        /// <returns>De hoeveelheid spelers</returns>
        public int GetAmountOfPlayers()
        {
            return this._allPlayers.Count();
        }

        /// <summary>
        /// Set de timer voor het starten van een spel
        /// </summary>
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

        /// <summary>
        /// Methode om het startcomando uit te voeren
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">EventArgs</param>
        private void startGame(Object source, ElapsedEventArgs e)
        {
            this.Start();

        }

        /// <summary>
        /// Geeft alle spelers in de game 
        /// </summary>
        /// <returns>Een collectie van InGamePlayerDTO's</returns>
        public IList<InGamePlayerDTO> GetPlayerNames()
        {
            IList<InGamePlayerDTO> names = new List<InGamePlayerDTO>();

            foreach (KeyValuePair<string, InGamePlayerDTO> player in this._allPlayers)
            {
                names.Add(player.Value);
            }

            return names;
        }

        /// <summary>
        /// Geeft een InGamePlayerDTO aan de hand van een username
        /// </summary>
        /// <param name="name">De naam van de speler waarvan je een InGamePlayerDTO wilt hebben</param>
        /// <returns>Een InGamePlayerDTO</returns>
        public InGamePlayerDTO GetPlayerObj(string name)
        {
            using (var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                InGamePlayerDTO player = _PlayerService.GetPlayerInGame(name);
                return player;
            }
        }

        /// <summary>
        /// Geeft alle categories in de game op als masteryDTO's, omdat je geen Dictionaries kan sturen
        /// </summary>
        /// <returns>Een collectie van MasteryDTO's</returns>
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