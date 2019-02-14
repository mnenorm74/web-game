using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace WebGame.Domain
{
    public class MongoTurnsRepository : ITurnsRepository
    {
        public const string CollectionName = "turns";
        private readonly IMongoCollection<GameTurnEntity> collection;

        public MongoTurnsRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<GameTurnEntity>(CollectionName);
            collection.Indexes.CreateOne(
                new CreateIndexModel<GameTurnEntity>(
                    Builders<GameTurnEntity>.IndexKeys
                        .Ascending(t => t.GameId).Ascending(t => t.TurnIndex))
            );
        }

        public IReadOnlyList<GameTurnEntity> GetLastTurns(Guid gameId, int maxCount)
        {
            return collection.Find(t => t.GameId == gameId).ToList();
        }

        public GameTurnEntity Insert(GameTurnEntity entity)
        {
            collection.InsertOne(entity);
            return entity;
        }
    }
}