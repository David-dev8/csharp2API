using System.Timers;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Models
{
    public class Game
    {
        private QuestionDTO _currentQuestion;
        private IList<InGamePlayerDTO> _allPlayers;
        private IDictionary<InGamePlayerDTO, int> _allResponses;
        private System.Timers.Timer _timer;
        public int _minimumPlayers { get; } = 3;
        public int _maximumPlayers { get; } = 5;
        private event EventHandler _questionEnded;
        private event EventHandler _playerWon;
        private IDictionary<CategoryDTO, float> _categories;
        private BoosterFactory _boosterFactory;
        private int _questionTimeInMili;
        private bool _inProgress;

        private IQuestionService _QuestionService;
        private IPlayerService _PlayerService;

        public Game(int questionTimeInMili, IQuestionService questionService, IPlayerService playerService)
        {
            this._currentQuestion = null;
            this._allPlayers = new List<InGamePlayerDTO>();
            this._allResponses = new Dictionary<InGamePlayerDTO, int>();
            this._categories = new Dictionary<CategoryDTO, float>();
            this._boosterFactory = new BoosterFactory();
            this._questionTimeInMili = questionTimeInMili;
            this._inProgress = false;

            this._QuestionService = questionService;
            this._PlayerService = playerService;

            IEnumerable<CategoryDTO> categories = _QuestionService.GetCategories();
            foreach (CategoryDTO cat in categories)
            {
                _categories.Add(cat, (float)100.0 / categories.Count());
            }
            _PlayerService = playerService;
        }

        private void SetTimer(int questionTimeInMili)
        {
            // Create a timer with a variable interval.
            _timer = new System.Timers.Timer(questionTimeInMili);
            // Hook up the Tick event for the timer. 
            _timer.Elapsed += this.NextQuestion;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void NextQuestion(Object source, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            int randomInt = rnd.Next(100);
            float counter = 0;

            foreach (CategoryDTO cat in this._categories.Keys)
            {
                counter += this._categories[cat];

                if (randomInt <= counter)
                {
                    this._currentQuestion = this._QuestionService.GetQuestionByCategoryId(cat.Id).ElementAt(rnd.Next(this._QuestionService.GetQuestionByCategoryId(cat.Id).Count())); // Get random element from array
                }
            }
        }

        public void AnswerQuestion(char id)
        {

        }

        public void UseBoost(string type)
        {
            _boosterFactory.getBooster(type).use(this);
        }

        public void EliminatePlayers()
        {

        }

        public bool CanStart()
        {
            if (this._allPlayers.Count >= this._minimumPlayers && this._allPlayers.Count <= this._maximumPlayers)
            {
                return true;
            }
            return false;
        }

        public void Start()
        {
            if (this.CanStart())
            {
                this.SetTimer(this._questionTimeInMili);
                this._inProgress = true;
            }
        }

        public bool CanJoin()
        {
            if (this._allPlayers.Count < this._maximumPlayers && this._inProgress == false)
            {
                return true;
            }
            return false;
        }

        public void Join(string name)
        {
            InGamePlayerDTO player = this._PlayerService.GetPlayerInGame(name);

            if (this.CanJoin())
            {
                this._allPlayers.Add(player);
            }
        }

        public int GetAmountOfPlayers()
        {
            return this._allPlayers.Count();
        }

    }
}
