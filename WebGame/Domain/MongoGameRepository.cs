using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace WebGame.Domain
{
    // TODO Сделате по аналогии с MongoUserRepository
    public class MongoGameRepository : IGameRepository
    {
        public const string CollectionName = "games";

        public MongoGameRepository(IMongoDatabase db)
        {
        }

        public GameEntity Insert(GameEntity game)
        {
            throw new NotImplementedException();
        }

        public GameEntity FindById(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public void Update(GameEntity game)
        {
            throw new NotImplementedException();
        }

        public IList<GameEntity> FindWaitingToStart(int limit)
        {
            throw new NotImplementedException();
        }

        public bool TryUpdateWaitingToStart(GameEntity game)
        {
            throw new NotImplementedException();
        }
    }
}