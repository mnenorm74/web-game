using System;

namespace WebGame.Domain
{
    public interface IGameRepository
    {
        GameEntity Insert(GameEntity game);
        GameEntity FindById(Guid gameId);
        void Update(GameEntity game);
    }
}