using System;
using JetBrains.Annotations;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity Create(UserEntity user);
        [CanBeNull]
        UserEntity FindById(Guid id);
        [NotNull]
        UserEntity GetOrCreateByLogin(string login);
        void Update(UserEntity user);
        UserEntity UpdateOrCreate(UserEntity user);
        void Delete(Guid id);
        PageList<UserEntity> GetPage(int pageNumber, int pageSize);
    }
}