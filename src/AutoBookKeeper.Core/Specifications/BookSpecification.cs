using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(Expression<Func<Book, bool>> criteria) : base(criteria)
    {
    }
}