using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<Guid, GameEntity> entities = new Dictionary<Guid, GameEntity>();

        public GameEntity Insert(GameEntity game)
        {
            if (game.Id != Guid.Empty)
                throw new Exception();

            var id = Guid.NewGuid();
            var entity = Clone(id, game);
            entities[id] = entity;
            return Clone(id, entity);
        }

        public GameEntity FindById(Guid id)
        {
            return entities.TryGetValue(id, out var entity) ? Clone(id, entity) : null;
        }

        public void Update(GameEntity game)
        {
            if (!entities.ContainsKey(game.Id))
                throw new InvalidOperationException();

            entities[game.Id] = Clone(game.Id, game);
        }

        private GameEntity Clone(Guid id, GameEntity game)
        {
            return new GameEntity(id, game.Status, game.TurnsCount, game.CurrentTurnIndex, game.Players.ToList());
        }
    }
}