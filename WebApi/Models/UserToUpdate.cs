using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserToUpdate
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}