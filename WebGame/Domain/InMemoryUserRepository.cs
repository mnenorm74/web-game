using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<string, UserEntity> entities = new Dictionary<string, UserEntity>();

        public UserEntity ReadById(string id)
        {
            return entities.TryGetValue(id, out var entity) ? entity : null;
        }

        public UserEntity ReadOrCreateUser(string name)
        {
            var existedUser = entities.Values.FirstOrDefault(u => u.Name == name);
            if (existedUser != null)
                return existedUser;

            var id = Guid.NewGuid().ToString();
            var user = new UserEntity(id, name);
            entities[id] = user;
            return user;
        }

        public void Update(UserEntity game)
        {
            entities[game.Id] = game;
        }
    }
}