using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Game.Domain
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
            userCollection.InsertOne(user);
            return user;
        }

        public UserEntity FindById(Guid id)
        {
            return userCollection.FindSync(userEntity => userEntity.Id == id).FirstOrDefault();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            var user = userCollection.FindSync(userEntity => userEntity.Login == login).FirstOrDefault();
            if(user != null)
            {
                return user;
            }
            var createdUser = new UserEntity(Guid.NewGuid());
            createdUser.Login = login;
            Insert(createdUser);
            return createdUser;
        }

        public void Update(UserEntity user)
        {
            userCollection.ReplaceOne(userEntity => userEntity.Id == user.Id, user);
        }

        public void Delete(Guid id)
        {
            userCollection.DeleteOne(userEntity => userEntity.Id == id);
        }

        // Для вывода списка всех пользователей (упорядоченных по логину)
        // страницы нумеруются с единицы
        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            var allUsers = userCollection.Find(new BsonDocument());
            var users = allUsers
                .SortBy(user => user.Login)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
        
                
            return new PageList<UserEntity>(users, userCollection.CountDocuments(new BsonDocument()), pageNumber, pageSize);
        }

        // Не нужно реализовывать этот метод
        public void UpdateOrInsert(UserEntity user, out bool isInserted)
        {
            throw new NotImplementedException();
        }
    }
}