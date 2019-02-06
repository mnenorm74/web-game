using System.Collections.Generic;

namespace WebGame.Domain
{
    public class GameEntity
    {
        public GameEntity(string id)
        {
            Id = id;
        }

        public string Id;
        public List<Player> Players;
        public int GamesCount;
        public int CurrentGameIndex;
    }
}