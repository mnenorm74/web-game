using System;
using MongoDB.Driver;

namespace WebGame.Domain
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;
        public const string CollectionName = "users";

        public MongoUserRepository(IMongoDatabase database)
        {
            userCollection = database.GetCollection<UserEntity>(CollectionName);
        }

        public UserEntity Insert(UserEntity user)
        {
            //TODO: Ищите в документации InsertXXX.
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

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        // Для вывода списка всех пользователей (упорядоченных по логину)
        // страницы нумеруются с единицы
        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            //TODO: Вам понадобятся Sort, Skip и Limit
            throw new NotImplementedException();
        }

        // Не нужно реализовывать этот метод
        public UserEntity UpdateOrInsert(UserEntity user)
        {
            throw new NotImplementedException();
        }

    }
}