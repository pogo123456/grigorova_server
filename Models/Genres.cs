using System.ComponentModel.DataAnnotations;

namespace Grigorova_Server.Models
{
    public class Genres
    {
        public int GenresId { get; set; }

        [Required]
        public string GenresName { get; set; } = string.Empty;
    }
}
