using System;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace WebGame.Domain
{
    public class UserEntity
    {
        [BsonConstructor]
        public UserEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; set; }
        public string CurrentGameId { get; set; } // Для того, чтобы использовать индекс по Game.Id, а не искать игру по индексу на Game.Players.UserId

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(CurrentGameId)}: {CurrentGameId}";
        }

        // Социальные сети, аватарки, ...
    }
}