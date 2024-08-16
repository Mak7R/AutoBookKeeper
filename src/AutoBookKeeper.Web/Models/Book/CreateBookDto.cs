using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.BookRules;

namespace AutoBookKeeper.Web.Models.Book;

public class CreateBookDto
{
    [Required]
    [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }
}