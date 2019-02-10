namespace WebGame.Domain
{
    public interface IGameRepository
    {
        GameEntity Create(GameEntity game);
        GameEntity ReadById(string gameId);
        void Update(GameEntity game);
    }
}