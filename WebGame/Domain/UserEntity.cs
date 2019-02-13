using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebGame.Domain
{
    public class UserEntity
    {
        public UserEntity()
        {
            Id = Guid.Empty;
        }

        public UserEntity(Guid id)
        {
            Id = id;
        }

        [BsonConstructor]
        public UserEntity(Guid id, string login, string lastName, string firstName, int gamesPlayed, Guid? currentGameId)
        {
            Id = id;
            Login = login;
            LastName = lastName;
            FirstName = firstName;
            GamesPlayed = gamesPlayed;
            CurrentGameId = currentGameId;
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

        // Социальные сети, аватарки, ...
    }
}