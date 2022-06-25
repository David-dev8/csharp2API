using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Enums;
using QuizRoyaleAPI.Services.Data;
using System.Timers;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is de game, alle logica en data met betrekking tot een game wordt hier afgehandelt.
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

        public QuestionDTO? CurrentQuestion { get; set; }
        public IDictionary<CategoryDTO, float> Categories { get; set; }
        public int MinimumPlayers { get; } = 11;
        public int MaximumPlayers { get; } = 2000;
        public System.Timers.Timer QuestionTimer { get; set; }

        public Game(int questionTimeInMili)
        {
            CurrentQuestion = null;
            _allPlayers = new Dictionary<string, InGamePlayerDTO>();
            _allResponses = new Dictionary<InGamePlayerDTO, char>();
            Categories = new Dictionary<CategoryDTO, float>();
            _boosterFactory = new BoosterFactory();
            _questionTimeInMili = questionTimeInMili;
            _inProgress = false;

            RegisterCategories();
        }

        // Haal alle categorieën op en sla deze op.
        private void RegisterCategories()
        {
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var _QuestionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                IEnumerable<CategoryDTO> categories = _QuestionService.GetCategories();
                foreach(CategoryDTO cat in categories)
                {
                    Categories.Add(cat, (float)100.0 / categories.Count());
                }
            }
        }

        /// <summary>
        /// Start de timer voor de antwoord tijd van een vraag.
        /// </summary>
        /// <param name="questionTime">Hoe lang de spelers hebben om te antwoorden in milliseconden.</param>
        // Set de timer voor de vraag
        public void SetTimer(int questionTime)
        {
            if(State.CurrentGame != null)
            {
                QuestionTimer = new System.Timers.Timer(questionTime);
                QuestionTimer.Elapsed += NextQuestion;
                QuestionTimer.AutoReset = false;
                QuestionTimer.Enabled = true;
                _miliStarted = GetCurrentMilis();
            }
        }

        /// <summary>
        /// Start een timer voor de cooldown tussen vragen in
        /// </summary>
        private void SetCooldownTimer()
        {
            if(State.CurrentGame != null)
            {
                // Create a timer with a variable interval.
                _coolDownTimer = new System.Timers.Timer(10000); // 10 seconden
                _coolDownTimer.Elapsed += StartNextQuestion;
                _coolDownTimer.AutoReset = false;
                _coolDownTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Start de volgende vraag.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">eventArgs</param>
        private void StartNextQuestion(object? source, ElapsedEventArgs e)
        {
            SetTimer(_questionTimeInMili);
            NotifyNextQuestion();
        }

        /// <summary>
        /// Laat iedereen weten dat de volgende vraag begint.
        /// </summary>
        /// <returns></returns>
        private async Task NotifyNextQuestion()
        {
            await State.GetHubContext().Clients.All.SendAsync("StartQuestion");// Documented
        }

        /// <summary>
        /// Kiest en stuurt de volgende vraag op. 
        /// Vragen worden gekozen op basis van een random categorie. 
        /// De kansen van deze random categorieen kunnen worden beïnvloed met boosters. 
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">EventArgs</param>
        private void NextQuestion(object? source, ElapsedEventArgs e)
        {
            Console.WriteLine("We gaan nu een nieuwe vraag kiezen");
            if(CurrentQuestion != null)
            {
                SendResultsFromLastQuestion();
            }

            Random random = new Random();
            int randomInt = random.Next(100);
            float counter = 0;

            foreach(CategoryDTO cat in Categories.Keys)
            {
                _allResponses = new Dictionary<InGamePlayerDTO, char>();
                counter += Categories[cat];

                if(randomInt <= counter)
                {
                    SelectQuestion(cat);
                    break;
                }
            }
        }

        // Selecteer een vraag voor de gegeven categorie
        private void SelectQuestion(CategoryDTO cat)
        {
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var _QuestionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                CurrentQuestion = _QuestionService.GetQuestionByCategoryId(cat.Id);
                CurrentQuestion.Category = cat;
                SendNewQuestion();
                SetCooldownTimer();
            }
        }

        /// <summary>
        /// Stuurt de inhoud van de volgende vraag op.
        /// </summary>
        /// <returns>stuurt de QuestionDTO op naar alle clients.</returns>
        public async Task SendNewQuestion()
        {
            await State.GetHubContext().Clients.All.SendAsync("newQuestion", CurrentQuestion);// Documented
            Console.WriteLine("----------------> " + CurrentQuestion.RightAnswer + " is het goede antwoord <-----------------");
        }

        /// <summary>
        /// Haalt de resultaten van de vorige vraag op om te sturen.
        /// </summary>
        private void SendResultsFromLastQuestion()
        {
            EliminatePlayers(GetPlayersToEliminate());
            Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn naa de purge");

            State.GetHubContext().Clients.All.SendAsync("playersLeft", _allPlayers.Values); // Doucumented

            if(_allPlayers.Count == 0)
            {
                EndTie();
            }
            else if(_allPlayers.Count == 1)
            {
                EndWin(_allPlayers.First().Value.Username);
            }
        }

        // Elimineer alle spelers met de gegeven collectie van connectie ids.
        private void EliminatePlayers(ICollection<string> playersToEliminate)
        {
            int finalPosition = _allPlayers.Count;
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                foreach(string id in playersToEliminate)
                {
                    _PlayerService.RegisterResult(_allPlayers[id].Username, Mode.QUIZ_ROYALE, finalPosition);
                    EliminatePlayer(id);
                }
            }
        }

        // Haal alle spelers op die geëlimineerd moeten worden.
        private ICollection<string> GetPlayersToEliminate()
        {
            if(_allResponses.Count > 0)
            {
                ICollection<string> playersToEliminate = new List<string>();
                using(var scope = State.ServiceProvider.CreateScope())
                {
                    var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                    foreach(KeyValuePair<string, InGamePlayerDTO> player in _allPlayers)
                    {
                        if(HasToBeEliminated(player.Value))
                        {
                            _PlayerService.RegisterAnswer(player.Value.Username, true, CurrentQuestion.Id);
                            _PlayerService.GiveRewards(player.Value.Username, QUESTION_XP, QUESTION_COINS);
                            SendResultToPlayer(player.Key, true, QUESTION_XP, QUESTION_COINS);
                            Console.WriteLine(player.Value.Username + " heeft het goed!");
                        }
                        else
                        {
                            _PlayerService.RegisterAnswer(player.Value.Username, false, CurrentQuestion.Id);
                            SendResultToPlayer(player.Key, false, 0, 0);
                            Console.WriteLine(player.Value.Username + " heeft geen goed antwoord gegegeven!");
                            playersToEliminate.Add(player.Key);
                        }
                    }
                }
                Console.WriteLine(_allPlayers.Count + " <-- dit is hoeveel spelers er over zijn voor de purge");
                return playersToEliminate;
            }
            else
            {
                return _allPlayers.Keys;
            }
        }

        // Controleert of een speler door mag gaan in het spel of geëlimineerd moet worden.
        private bool HasToBeEliminated(InGamePlayerDTO player)
        {
            return _allResponses.ContainsKey(player)
                && (_allResponses[player] == CurrentQuestion.RightAnswer || _allResponses[player] == '*');
        }

        /// <summary>
        /// Eindigd het spel in gelijkspel. 
        /// Dit kan voorkomen als alle spelers een vraag fout of niet beantwoorden.
        /// </summary>
        /// <returns></returns>
        // Als alle spelers af zijn is er geen winner en is de game voorbij.
        public async Task EndTie()
        {
            State.CurrentGame = null;
            RemoveTimers();
            Console.WriteLine("gelijkspelgelijkspelgelijkspelgelijkspelgelijkspelgelijkspel");
        }

        /// <summary>
        /// Eindigt het spel in een winst voor een speler.
        /// </summary>
        /// <param name="winnerName">De username van de speler die heeft gewonnen.</param>
        /// <returns></returns>
        // Als er maar 1 speler over is dan wint deze speler.
        public async Task EndWin(string winnerName)
        {
            using(var scope = State.ServiceProvider.CreateScope())
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
        /// Collect alle timers om het spel netjes af te sluiten.
        /// </summary>
        private void RemoveTimers()
        {
            QuestionTimer.Elapsed -= NextQuestion;
            QuestionTimer.Elapsed -= StartNextQuestion;
            _startDelayTimer.Elapsed -= startGame;
            QuestionTimer.Stop();
            _startDelayTimer.Stop();
            QuestionTimer.Dispose();
            _startDelayTimer.Dispose();
        }

        /// <summary>
        /// Stuurt de resultaten van de vorige vraag op.
        /// </summary>
        /// <param name="conectionId">De conectie waarnaa het resultaat moet worden gestuurd.</param>
        /// <param name="result">Het resultaat dat de speler heeft.</param>
        /// <param name="xp">Het aantal XP dat de speler heeft verdient met dit resultaat.</param>
        /// <param name="coins">Het aantal coins dat de speler heeft verdient met dit resultaat.</param>
        /// <returns>Geeft de client een event met zijn resultaten.</returns>
        private async Task SendResultToPlayer(string conectionId, bool result, int xp, int coins)
        {
            await State.GetHubContext().Clients.Client(conectionId).SendAsync("result", result, xp, coins); // Documented
        }

        /// <summary>
        /// Registreert een antwoord van een speler.
        /// </summary>
        /// <param name="id">De answerID van de awnser die de speler heeft gegeven.</param>
        /// <param name="conectionId">De conectie van de speler die het resultaat stuurt.</param>
        /// <returns>Stuurt alle clients een event dat iemand heeft geantwoord met de tijd erbij.</returns>
        public async Task AnswerQuestion(char id, string conectionId)
        {
            InGamePlayerDTO player = _allPlayers[conectionId];
            if(!_allResponses.ContainsKey(player))
            {
                _allResponses.Add(player, id);
                await State.GetHubContext().Clients.All.SendAsync("playerAnswered", player, (GetCurrentMilis() - _miliStarted) / 1000.0);
            }
        }

        // Haalt de huidige timestamp in milliseconden op.
        private long GetCurrentMilis()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Gebruikt een boost.
        /// </summary>
        /// <param name="type">De type Booster die je wilt gebruiken.</param>
        /// <param name="options">De opties voor de booster.</param>
        /// <param name="conncectionId">De connectieID van de speler.</param>
        public void UseBoost(string type, string options, string conncectionId)
        {
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                playerService.removeItem(_allPlayers[conncectionId].Username, type);
            }
            _boosterFactory.GetBooster(type).Use(this, options);
            Console.WriteLine("De kansen zijn Nu: ");
            float totaal = 0;
            foreach(KeyValuePair<CategoryDTO, float> chance in Categories)
            {
                Console.WriteLine(chance.Key.Name + " Heeft nu een kans van " + chance.Value + "%");
                totaal += chance.Value;
                Console.WriteLine("Totaal is nu " + totaal + "%");
            }
        }

        /// <summary>
        /// Elimineert een speler uit een game.
        /// </summary>
        /// <param name="playerID">De connectie van de speler die moet worden geëlimineerd.</param>
        /// <returns>Geeft de geëlimineerde speler een event om te laten weten dat hij af is.</returns>
        public async Task EliminatePlayer(string playerID)
        {
            _allPlayers.Remove(playerID);
            await State.GetHubContext().Clients.Client(playerID).SendAsync("gameOver"); // Documented
        }

        /// <summary>
        /// Kijkt of het mogelijk is om een spel te starten.
        /// </summary>
        /// <returns>True als een spel gestart kan worden, anders false.</returns>
        public bool CanStart()
        {
            if(_allPlayers.Count >= MinimumPlayers && _allPlayers.Count <= MaximumPlayers && _inProgress == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Start een game.
        /// </summary>
        /// <returns>Stuurt alle clients een event dat het spel is gestart.</returns>
        public async Task Start()
        {
            if(CanStart())
            {
                Console.WriteLine("De game gaat nu starten");
                await State.GetHubContext().Clients.All.SendAsync("start"); // Documented
                SetTimer(10);
                _inProgress = true;
            }
        }

        /// <summary>
        /// Kijkt of het mogelijk is voor en nieuwe speler om te joinen.
        /// </summary>
        /// <returns>True als de speler kan joinen, anders false.</returns>
        public bool CanJoin()
        {
            if(_allPlayers.Count < MaximumPlayers && _inProgress == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Laat een speler joinen.
        /// </summary>
        /// <param name="name">De naam van de speler die joint.</param>
        /// <param name="conectionId">De connectie van de speler die joint.</param>
        public void Join(string name, string conectionId)
        {
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                InGamePlayerDTO player = _PlayerService.GetPlayerInGame(name);

                if(CanJoin())
                {
                    _allPlayers.Add(conectionId, player);

                    if(CanStart())
                    {
                        SetStartTimer();
                    }
                }
            }
        }

        /// <summary>
        /// Haalt de hoeveelheid spelers in de game op.
        /// </summary>
        /// <returns>De hoeveelheid spelers.</returns>
        public int GetAmountOfPlayers()
        {
            return _allPlayers.Count();
        }

        /// <summary>
        /// Set de timer voor het starten van een spel.
        /// </summary>
        private void SetStartTimer()
        {
            if(_startDelayTimer != null)
            {
                // Reset de timer als en nieuwe deelnemer binnekomt.
                _startDelayTimer.Stop();
                _startDelayTimer.Start();
            }
            else
            {
                _startDelayTimer = new System.Timers.Timer(5000);
                _startDelayTimer.Elapsed += startGame;
                _startDelayTimer.AutoReset = false;
                _startDelayTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Methode om het startcomando uit te voeren.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="e">EventArgs</param>
        private void startGame(Object source, ElapsedEventArgs e)
        {
            Start();

        }

        /// <summary>
        /// Geeft alle spelers in de game. 
        /// </summary>
        /// <returns>Een collectie van InGamePlayerDTO's.</returns>
        public IList<InGamePlayerDTO> GetPlayerNames()
        {
            IList<InGamePlayerDTO> names = new List<InGamePlayerDTO>();

            foreach(KeyValuePair<string, InGamePlayerDTO> player in _allPlayers)
            {
                names.Add(player.Value);
            }

            return names;
        }

        /// <summary>
        /// Geeft een InGamePlayerDTO aan de hand van een username.
        /// </summary>
        /// <param name="name">De naam van de speler waarvan je een InGamePlayerDTO wilt hebben.</param>
        /// <returns>Een InGamePlayerDTO</returns>
        public InGamePlayerDTO GetPlayerObj(string name)
        {
            using(var scope = State.ServiceProvider.CreateScope())
            {
                var _PlayerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                InGamePlayerDTO player = _PlayerService.GetPlayerInGame(name);
                return player;
            }
        }

        /// <summary>
        /// Geeft alle categories in de game op als masteryDTO's, omdat je geen Dictionaries kan sturen.
        /// </summary>
        /// <returns>Een collectie van MasteryDTO's.</returns>
        public IList<CategoryIntensityDTO> GetCategories()
        {
            IList<CategoryIntensityDTO> list = new List<CategoryIntensityDTO>();
            foreach(KeyValuePair<CategoryDTO, float> cat in Categories)
            {
                list.Add(new CategoryIntensityDTO(cat.Key, cat.Value));
            }
            return list;
        }
    }
}