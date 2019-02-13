using System;

namespace WebGame.Domain
{
    public class UserEntity
    {
        public UserEntity(Guid id)
        {
            Id = id;
        }

        public Guid Id
        {
            get;
            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local For MongoDB
            private set;
        }
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int GamesPlayed { get; set; }
        public Guid? CurrentGameId { get; set; } // Для того, чтобы использовать индекс по Game.Id, а не искать игру по индексу на Game.Players.UserId

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Login)}: {Login}, {nameof(CurrentGameId)}: {CurrentGameId}";
        }

        public void FinishGame()
        {
            GamesPlayed++;
            CurrentGameId = null;
        }
    }
}