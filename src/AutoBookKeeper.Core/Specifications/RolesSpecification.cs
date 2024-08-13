using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class RolesSpecification : BaseSpecification<Role>
{
    public RolesSpecification()
    {
    }
    
    public RolesSpecification(Expression<Func<Role, bool>> criteria) : base(criteria)
    {
    }
}