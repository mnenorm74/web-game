using System;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity FindById(Guid id);
        UserEntity GetOrCreateByLogin(string login);
        void Update(UserEntity user);
    }
}