using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class UserSpecification : BaseSpecification<User>
{
    public UserSpecification()
    {
    }
    
    public UserSpecification(Expression<Func<User, bool>> criteria) : base(criteria)
    {
    }
}