using System.ComponentModel.DataAnnotations;
using AutoBookKeeper.Core.Entities.Base;

using static AutoBookKeeper.Core.Rules.Length.TransactionTypeStrings;

namespace AutoBookKeeper.Core.Entities;

public class TransactionType : Entity<Guid>
{
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }
    
    public Book Book { get; set; }
}