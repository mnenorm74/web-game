using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class PlayerToUpdate
    {
        [Required]
        public string Name { get; set; }
    }
}