using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace WebGame.Domain
{
    public class MongoUserRepositoty : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;
        public const string CollectionName = "users";

        public MongoUserRepositoty(IMongoDatabase database)
        {
            userCollection = database.GetCollection<UserEntity>(CollectionName);

            // Индекс для быстрого поиска по совпадению логина
            var indexModel = new CreateIndexModel<UserEntity>(
                Builders<UserEntity>.IndexKeys.Ascending(f => f.Login), new CreateIndexOptions { Unique = true });
            userCollection.Indexes.CreateOne(indexModel);
        }

        public void Create(UserEntity user)
        {
            userCollection.InsertOne(user);
        }

        public UserEntity FindById(Guid id)
        {
            return userCollection.Find(u => u.Id == id).SingleOrDefault();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            // Возможен data-race двух параллельных запросов GetOrCreate
            // В один запрос c Upsert-ом
            try
            {
                return userCollection.FindOneAndUpdate<UserEntity>(
                    u => u.Login == login,
                    Builders<UserEntity>.Update
                        .SetOnInsert(u => u.Id, Guid.NewGuid()),
                    new FindOneAndUpdateOptions<UserEntity, UserEntity>
                    {
                        IsUpsert = true,
                        ReturnDocument = ReturnDocument.After
                    });

            }
            catch (MongoCommandException e) when (e.Code == 11000)
            {
                return userCollection.FindSync(u => u.Login == login).First();
            }            
            //Без изысков в два запроса. 
            var userEntity = userCollection.FindSync(u => u.Login == login).FirstOrDefault();
            if (userEntity != null) return userEntity;
            var newUser = new UserEntity(Guid.NewGuid()){Login = login};
            userCollection.InsertOne(newUser);
            return newUser;
        }

        public void Update(UserEntity user)
        {
            userCollection.ReplaceOne(u => u.Id == user.Id, user);
        }

        public void UpdateOrCreate(UserEntity user)
        {
            userCollection.ReplaceOne(u => u.Id == user.Id, user, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public void Delete(Guid id)
        {
            userCollection.DeleteOne(u => u.Id == id);
        }

        // Частичное обновление в батч-запросе
        public void UpdatePlayersWhenGameIsFinished(IEnumerable<Guid> userIds)
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