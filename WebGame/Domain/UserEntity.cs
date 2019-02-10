using System;
using System.Text;

namespace WebGame.Domain
{
    public class UserEntity
    {
        public UserEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public UserEntity(string name)
        {
            Name = name;
        }

        public string Id { get; }
        public string Name { get; set; }
        public string CurrentGameId { get; set; } // Для того, чтобы использовать индекс по Game.Id, а не искать игру по индексу на Game.Players.UserId && 

        // Социальные сети, аватарки, ...
    }
}