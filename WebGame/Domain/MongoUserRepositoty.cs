using System;
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
        }

        public UserEntity Create(UserEntity user)
        {
            //TODO: Ищите в документации InsertXXX
            throw new NotImplementedException();
        }

        public UserEntity FindById(Guid id)
        {
            //TODO: Ищите в документации FindXXX
            throw new NotImplementedException();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            //TODO: Это Find или Insert
            throw new NotImplementedException();
        }

        public void Update(UserEntity user)
        {
            //TODO: Ищите в документации ReplaceXXX
            throw new NotImplementedException();
        }

        public UserEntity UpdateOrCreate(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}