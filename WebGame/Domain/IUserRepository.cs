using System;
using JetBrains.Annotations;

namespace WebGame.Domain
{
    public interface IUserRepository
    {
        void Create(UserEntity user);
        [CanBeNull]
        UserEntity FindById(Guid id);
        [NotNull]
        UserEntity GetOrCreateByLogin(string login);
        void Update(UserEntity user);
        void UpdateOrCreate(UserEntity user);
        void Delete(Guid id);
    }
}