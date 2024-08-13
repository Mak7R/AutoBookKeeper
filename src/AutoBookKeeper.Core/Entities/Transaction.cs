using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBookKeeper.Core.Entities.Base;

using static AutoBookKeeper.Core.Rules.Length.TransactionStrings;

namespace AutoBookKeeper.Core.Entities;

public class Transaction : Entity<Guid>
{
    [StringLength(MaxNameIdentifierLength, MinimumLength = MinNameIdentifierLength)]
    public string NameIdentifier { get; set; } = string.Empty;
    
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }
    
    public DateTime TransactionTime { get; set; }
    
    [ForeignKey(nameof(Type))]
    public Guid TypeId { get; set; }
    public TransactionType Type { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}