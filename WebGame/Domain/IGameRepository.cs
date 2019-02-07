using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public interface IGameRepository
    {
        GameEntity Insert(GameEntity game);
        GameEntity GetById(string gameId);
        void Update(GameEntity game);
    }

    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<GameEntity> items = new List<GameEntity>();
        private int nextId;

        public GameEntity Insert(GameEntity game)
        {
            var id = nextId++;
            game.Id = id.ToString();
            items.Add(game);
            return game;
        }

        public GameEntity GetById(string gameId)
        {
            return items.First(g => g.Id == gameId);
        }

        public void Update(GameEntity game)
        {
            items.RemoveAll(g => g.Id == game.Id);
            items.Add(game);
        }
    }
}