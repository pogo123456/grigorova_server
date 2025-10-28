using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Grigorova_Server.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        public string BookTitle { get; set; } = string.Empty;
        public int? GenreId { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательной")]
        public decimal BookPrice { get; set; }

        [Range(0, 5, ErrorMessage = "Рейтинг должен быть от 0 до 5")]
        [Display(Name = "Book Rating")]
        public decimal BookRating { get; set; } = 0;

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
