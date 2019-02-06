namespace WebGame.Domain
{
    public class Player
    {
        public Player(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public string UserId { get; }
        public string Name { get; } // Снэпшот имени на момент старта игры. Может быть такое требование!
        public PlayerDecision Decision { get; set; }
        public int Score { get; set; }
    }
}