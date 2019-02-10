using System;
using System.Collections.Generic;

namespace WebGame.Domain
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<string, GameEntity> entities = new Dictionary<string, GameEntity>();

        public GameEntity Create(GameEntity game)
        {
            var id = Guid.NewGuid().ToString();
            game.Id = id;
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