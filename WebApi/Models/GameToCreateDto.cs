using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class GameToCreateDto
    {
        [Required]
        public int TurnsCount { get; set; }
    }
}