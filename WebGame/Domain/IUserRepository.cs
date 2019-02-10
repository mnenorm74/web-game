namespace WebGame.Domain
{
    public interface IUserRepository
    {
        UserEntity ReadById(string id);
        UserEntity ReadOrCreateUser(string name);
        void Update(UserEntity user);
    }
}