
namespace AutoBookKeeper.Web.Models.Book;

public class BookViewModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreationTime { get; set; }
}