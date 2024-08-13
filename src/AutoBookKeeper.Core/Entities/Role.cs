using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.ValueObjects;

using static AutoBookKeeper.Core.Rules.Length.RoleStrings;

namespace AutoBookKeeper.Core.Entities;

public class Role : Entity<Guid>
{
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;
    
    public BookAccess Access { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
