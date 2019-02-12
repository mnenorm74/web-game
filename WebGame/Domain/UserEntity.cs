using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebGame.Domain
{
    public class UserEntity
    {
        [BsonConstructor]
        public UserEntity(Guid id)
        {
            Id = id;
        }

        [BsonId]
        public Guid Id
        {
            get;
            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local For MongoDB
            private set;
        }
        public string Login { get; set; }
        public int GamesPlayed { get; set; }
        public Guid? CurrentGameId { get; set; } // Для того, чтобы использовать индекс по Game.Id, а не искать игру по индексу на Game.Players.UserId

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Login)}: {Login}, {nameof(CurrentGameId)}: {CurrentGameId}";
        }

        // Социальные сети, аватарки, ...
    }
}