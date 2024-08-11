using System.ComponentModel.DataAnnotations;
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
    
    public TransactionType Type { get; set; }
    
    public Book Book { get; set; }
}