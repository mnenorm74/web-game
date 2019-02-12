using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserToCreate
    {
        [Required]
        public string UserName { get; set; }

        [DefaultValue("John")]
        public string FirstName { get; set; }

        [DefaultValue("Doe")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}