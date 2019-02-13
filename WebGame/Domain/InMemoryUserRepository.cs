using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Guid adminId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        private const string AdminLogin = "Admin";
        private readonly Dictionary<Guid, UserEntity> entities = new Dictionary<Guid, UserEntity>();

        public InMemoryUserRepository()
        {
            AddAdmin();
        }

        private void AddAdmin()
        {
            var user = new UserEntity(adminId) {Login = AdminLogin};
            entities[user.Id] = user;
        }

        public void Create(UserEntity user)
        {
            entities[user.Id] = user;
        }

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

        public void Update(UserEntity user)
        {
            if (entities.ContainsKey(user.Id))
                entities[user.Id] = user;
        }

        public void Delete(Guid id)
        {
            entities.Remove(id);
        }

        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            var count = entities.Count;
            var items = entities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(pair => pair.Value)
                .ToList();
            return new PageList<UserEntity>(items, count, pageNumber, pageSize);
        }
    }
}