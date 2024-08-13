using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBookKeeper.Core.Entities.Base;
using static AutoBookKeeper.Core.Rules.Length.BookStrings;
namespace AutoBookKeeper.Core.Entities;


public class Book : Entity<Guid>
{
    [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }
    
    [ForeignKey(nameof(Owner))]
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    
    public DateTime CreationTime { get; set; }
}
