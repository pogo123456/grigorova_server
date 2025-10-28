using Grigorova_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class BookAuthor
{
    public int BookAuthorId { get; set; }

    public int BookId { get; set; }
    [JsonIgnore]
    public Book? Book { get; set; }

    public int AuthorId { get; set; }
    [JsonIgnore]
    public Author? Author { get; set; }
}
