using System;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity Create(UserEntity user);
        UserEntity FindById(Guid id);
        UserEntity GetOrCreateByLogin(string login);
        void Update(UserEntity user);
        UserEntity UpdateOrCreate(UserEntity user);
        void Delete(Guid id);
        PageList<UserEntity> GetPage(int pageNumber, int pageSize);
    }
}