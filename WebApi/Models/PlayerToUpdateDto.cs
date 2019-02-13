using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class PlayerToUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}