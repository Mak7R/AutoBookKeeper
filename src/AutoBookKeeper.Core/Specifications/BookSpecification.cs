using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    private BookSpecification(Expression<Func<Book, bool>> criteria) : base(criteria)
    {
    }

    public static BookSpecification GetUserBooks(Guid userId)
    {
        return new BookSpecification(b => b.OwnerId == userId);
    }
}