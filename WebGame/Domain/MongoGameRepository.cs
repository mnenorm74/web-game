using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace WebGame.Domain
{
    // TODO Сделать по аналогии с MongoUserRepository
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
            //TODO: Реализуй метод, который возвращает не более чем limit игр со статусом GameStatus.WaitingToStart
            throw new NotImplementedException();
        }

        public bool TryUpdateWaitingToStart(GameEntity game)
        {
            //TODO: Реализуй метод, который обновляет игру, если она находится в статусе GameStatus.WaitingToStart
            // Для проверки успешности используй IsAcknowledged и ModifiedCount из результата.
            throw new NotImplementedException();
        }
    }
}