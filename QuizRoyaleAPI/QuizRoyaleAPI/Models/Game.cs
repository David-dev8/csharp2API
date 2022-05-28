using System.Timers;

namespace QuizRoyaleAPI.Models
{
    public class Game
    {
        private Question _currentQuestion;
        private IList<Player> _allPlayers;
        private IDictionary<Player, int> _allResponses;
        private System.Timers.Timer _timer;
        private int _minimumPlayers = 5;
        private int _maximumPlayers = 10;
        private event EventHandler _questionEnded;
        private event EventHandler _playerWon;
        private IDictionary<Category, float> _categories;
        private BoosterFactory _boosterFactory;
        private int _questionTimeInMili;

        public Game(int questionTimeInMili)
        {
            this._currentQuestion = null;
            this._allPlayers = new List<Player>();
            this._allResponses = new Dictionary<Player, int>();
            this._categories = new Dictionary<Category, float>();
            this._boosterFactory = new BoosterFactory();
            this._questionTimeInMili = questionTimeInMili;

        }

        private void SetTimer(int questionTimeInMili)
        {
            // Create a timer with a variable interval.
            _timer = new System.Timers.Timer(questionTimeInMili);
            // Hook up the Tick event for the timer. 
            _timer.Elapsed += this.TimerTick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void TimerTick(Object source, ElapsedEventArgs e)
        { 
            
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
            }
        }

    }
}
