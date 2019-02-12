using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace WebGame.Domain
{
    public class MongoUserRepositoty2 : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;

        public MongoUserRepositoty2(IMongoDatabase database)
        {
            userCollection = database.GetCollection<UserEntity>("users");
        }

        public UserEntity ReadById(string id)
        {
            //см userCollection.FindЧегоТоТам
            throw new NotImplementedException();
        }

        public UserEntity ReadOrCreateUser(string id)
        {
            //см userCollection.InsertЧегоТоТам
            throw new NotImplementedException();
        }

        public void Update(UserEntity user)
        {
            //см userCollection.ReplaceЧегоТоТам
            throw new NotImplementedException();
        }
    }

    public class MongoUserRepositoty : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;
        public const string CollectionName = "users";

        public MongoUserRepositoty(IMongoDatabase database)
        {
            userCollection = database.GetCollection<UserEntity>(CollectionName);
        }

        public UserEntity ReadById(string id)
        {
            return userCollection.Find(u => u.Id == id).Single();
        }

        public UserEntity ReadOrCreateUser(string id)
        {
            return userCollection.FindOneAndUpdate<UserEntity>(
                u => u.Id == id, 
                Builders<UserEntity>.Update.SetOnInsert(u => u.Name, "Anonymous"), 
                new FindOneAndUpdateOptions<UserEntity, UserEntity>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                });
            //var userEntity = userCollection.FindSync(u => u.Id == id).FirstOrDefault();
            //if (userEntity != null) return userEntity;
            //var newUser = new UserEntity(id, "");
            //userCollection.InsertOne(newUser);
            //return newUser;
        }

        public void Update(UserEntity user)
        {
            userCollection.ReplaceOne(u => u.Id == user.Id, user);
        }

        // Атомарное обновление
        public void UpdatePlayersWhenGameIsFinished(IEnumerable<string> userIds)
        {
            var updateBuilder = Builders<UserEntity>.Update;
            userCollection.UpdateMany(
                Builders<UserEntity>.Filter.In(u => u.Id, userIds),
                updateBuilder.Combine(
                    updateBuilder.Inc(u => u.GamesPlayed, 1),
                    updateBuilder.Set(u => u.CurrentGameId, null)
                ));
        }
    }
}