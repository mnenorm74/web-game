namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity ReadById(string id);
        UserEntity ReadOrCreateUser(string id);
        void Update(UserEntity user);
    }
}