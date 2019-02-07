using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity GetById(string id);
        UserEntity GetOrCreateUser(string name);
        void Update(UserEntity user);
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<UserEntity> entities = new List<UserEntity>();
        private int nextId;

        public UserEntity GetById(string id)
        {
            return entities.First(u => u.Id == id);
        }

        public UserEntity GetOrCreateUser(string name)
        {
            var existedUser = entities.FirstOrDefault(u => u.Name == name);
            if (existedUser != null) return existedUser;
            var id = nextId++;
            var user = new UserEntity(id.ToString(), name);
            entities.Add(user);
            return user;
        }

        public void Update(UserEntity entity)
        {
            entities.RemoveAll(g => g.Id == entity.Id);
            entities.Add(entity);
        }
    }
}