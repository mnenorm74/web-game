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

        public string Id { get; }
        public string Name { get; set; }
        // Социальные сети, аватарки, ...
    }
}
