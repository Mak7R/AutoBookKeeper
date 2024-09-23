using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class RolesSpecification : BaseSpecification<BookRole>
{
    public RolesSpecification()
    {
    }
    
    public RolesSpecification(Expression<Func<BookRole, bool>> criteria) : base(criteria)
    {
    }
}