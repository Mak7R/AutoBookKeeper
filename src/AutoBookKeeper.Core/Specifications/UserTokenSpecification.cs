using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class UserTokenSpecification : BaseSpecification<UserToken>
{
    private UserTokenSpecification(Expression<Func<UserToken, bool>> criteria) : base(criteria)
    {
    }

    public static UserTokenSpecification TokensByUserId(Guid userId)
    {
        return new UserTokenSpecification(t => t.UserId == userId);
    }
}