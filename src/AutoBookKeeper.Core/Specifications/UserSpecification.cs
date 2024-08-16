using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class UserSpecification : BaseSpecification<User>
{
    private UserSpecification(Expression<Func<User, bool>> criteria) : base(criteria)
    {
    }

    public static UserSpecification GetByName(string name)
    {
        return new UserSpecification(u => u.UserName == name);
    }

    public static UserSpecification GetByEmail(string email)
    {
        return new UserSpecification(u => u.Email == email);
    }
}