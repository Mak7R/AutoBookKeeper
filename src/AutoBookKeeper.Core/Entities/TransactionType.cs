using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBookKeeper.Core.Entities.Base;

using static AutoBookKeeper.Core.Rules.Length.TransactionTypeStrings;

namespace AutoBookKeeper.Core.Entities;

public class TransactionType : Entity<Guid>
{
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}