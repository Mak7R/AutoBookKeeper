using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class TransactionTypeSpecification : BaseSpecification<TransactionType>
{
    private TransactionTypeSpecification(Expression<Func<TransactionType, bool>> criteria) : base(criteria)
    {
    }

    public static ISpecification<TransactionType> GetBookTransactionTypes(Guid bookId)
    {
        return new TransactionTypeSpecification(tt => tt.BookId == bookId);
    }
}