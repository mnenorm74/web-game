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

        public UserEntity Create(UserEntity user)
        {
            //см userCollection.InsertЧегоТоТам
            throw new NotImplementedException();
        }

        public UserEntity FindById(Guid id)
        {
            //см userCollection.FindЧегоТоТам
            throw new NotImplementedException();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            //см userCollection.InsertЧегоТоТам
            throw new NotImplementedException();
        }

        public void Update(UserEntity user)
        {
            //см userCollection.ReplaceЧегоТоТам
            throw new NotImplementedException();
        }

        public UserEntity UpdateOrCreate(UserEntity user)
        {
            //см userCollection.ReplaceЧегоТоТам
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            //см userCollection.DeleteЧегоТоТам
            throw new NotImplementedException();
        }

        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
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

        public UserEntity Create(UserEntity user)
        {
            userCollection.InsertOne(user);
            return user;
        }

        public UserEntity FindById(Guid id)
        {
            return userCollection.Find(u => u.Id == id).SingleOrDefault();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            return userCollection.FindOneAndUpdate<UserEntity>(
                u => u.Login == login, 
                Builders<UserEntity>.Update.SetOnInsert(u => u.Id, Guid.NewGuid()), 
                new FindOneAndUpdateOptions<UserEntity, UserEntity>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                });
            //var userEntity = userCollection.FindSync(u => u.Login == login).FirstOrDefault();
            //if (userEntity != null) return userEntity;
            //var newUser = new UserEntity(){Login = login};
            //userCollection.InsertOne(newUser);
            //return newUser;
        }

        public void Update(UserEntity user)
        {
            userCollection.ReplaceOne(u => u.Id == user.Id, user);
        }

        public UserEntity UpdateOrCreate(UserEntity user)
        {
            userCollection.ReplaceOne(u => u.Id == user.Id, user, new UpdateOptions
            {
                IsUpsert = true
            });
            return user;
        }

        public void Delete(Guid id)
        {
            userCollection.DeleteOne(u => u.Id == id);
        }

        // Медленная, но простая реализация пейджинга для MongoDB
        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            var items = userCollection
                .Find(x => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            var count = userCollection.CountDocuments(x => true);
            return new PageList<UserEntity>(items, count, pageNumber, pageSize);
        }

        // Атомарное обновление
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