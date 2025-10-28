using System.ComponentModel.DataAnnotations;

namespace Grigorova_Server.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required]
        public string AuthorFirstname { get; set; } = string.Empty;

        [Required]
        public string AuthorSecondname { get; set; } = string.Empty;

        [Display(Name = "Author Birthday")]
        [DataType(DataType.Date)]
        public DateTime AuthorBirthday { get; set; }

        [Required]
        public string AuthorCountry { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Рейтинг должен быть от 0 до 5")]
        [Display(Name = "Author Rating")]
        public decimal AuthorRating { get; set; } = 0;

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
