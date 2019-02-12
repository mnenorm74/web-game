using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, UserEntity> entities = new Dictionary<Guid, UserEntity>();

        public UserEntity FindById(Guid id)
        {
            return entities.TryGetValue(id, out var entity) ? entity : null;
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            var existedUser = entities.Values.FirstOrDefault(u => u.Login == login);
            if (existedUser != null)
                return existedUser;

            var user = new UserEntity(Guid.NewGuid());
            entities[user.Id] = user;
            return user;
        }

        public void Update(UserEntity game)
        {
            entities[game.Id] = game;
        }
    }
}