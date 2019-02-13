using System;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        void Create(UserEntity user);
        UserEntity FindById(Guid id);
        UserEntity GetOrCreateByLogin(string login);
        void Update(UserEntity user);
        void UpdateOrCreate(UserEntity user);
        void Delete(Guid id);
    }
}