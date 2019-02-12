using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<string, GameEntity> entities = new Dictionary<string, GameEntity>();

        public GameEntity Create(GameEntity game)
        {
            var id = Guid.NewGuid().ToString();
            game = new GameEntity(id, game.Status, game.TurnsCount, game.CurrentTurnIndex, game.Players.ToList());
            entities[id] = game;
            return game;
        }

        public GameEntity ReadById(string id)
        {
            return entities.TryGetValue(id, out var entity) ? entity : null;
        }

        public void Update(GameEntity game)
        {
            entities[game.Id] = game;
        }
    }
}