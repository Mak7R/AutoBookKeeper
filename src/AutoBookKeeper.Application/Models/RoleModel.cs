using System.ComponentModel.DataAnnotations.Schema;
using AutoBookKeeper.Application.Models.Base;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.ValueObjects;

namespace AutoBookKeeper.Application.Models;

public class RoleModel : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public BookAccess Access { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
}