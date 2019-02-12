using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<Guid, GameEntity> entities = new Dictionary<Guid, GameEntity>();

        public GameEntity Create(GameEntity game)
        {
            var id = Guid.NewGuid();
            game = new GameEntity(id, game.Status, game.TurnsCount, game.CurrentTurnIndex, game.Players.ToList());
            entities[id] = game;
            return game;
        }

        public GameEntity FindById(Guid id)
        {
            return entities.TryGetValue(id, out var entity) ? entity : null;
        }

        public void Update(GameEntity game)
        {
            entities[game.Id] = game;
        }
    }
}