using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebGame.Domain
{
    public class Player
    {
        [BsonConstructor]
        public Player(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        [BsonElement("userId")]
        public Guid UserId { get; }

        [BsonElement("name")]
        public string Name { get; } // Снэпшот имени на момент старта игры. Может быть такое требование!
        public PlayerDecision Decision { get; set; }
        public int Score { get; set; }
    }
}