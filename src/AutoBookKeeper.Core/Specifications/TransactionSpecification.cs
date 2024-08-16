using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class TransactionSpecification : BaseSpecification<Transaction>
{
    private TransactionSpecification(Expression<Func<Transaction, bool>> criteria) : base(criteria)
    {
    }

    public static ISpecification<Transaction> GetBookTransactions(Guid bookId)
    {
        return new TransactionSpecification(t => t.BookId == bookId);
    }
}