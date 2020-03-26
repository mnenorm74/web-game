using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CreatingUserDto
    {        
        [Required]
        public string Login { get; set; }
        [DefaultValue("John")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}