namespace QuizRoyaleAPI.Models
{
    public class Game
    {
        private Question currentQuestion;
        private IList<Player> allPlayers;
        private IDictionary<Player, int> allResponses;
        private Timer timer;
        private static int minimumPlayers = 5;
        private event EventHandler questionEnded;
        private event EventHandler playerWon;
        private IDictionary<Category, float> categories;

        public void answerQuestion(char id) 
        {

        }

        public void useBoost(string type) 
        {

        }

        public void eliminatePlayers() 
        {

        }

        public bool canStart() 
        {

        }

        public void start() 
        {
            if (this.canStart())
            { 
                
            }
        }

    }
}
