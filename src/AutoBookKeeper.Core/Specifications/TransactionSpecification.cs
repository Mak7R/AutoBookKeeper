using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class TransactionSpecification : BaseSpecification<Transaction>
{
    public TransactionSpecification()
    {
    }
    
    public TransactionSpecification(Expression<Func<Transaction, bool>> criteria) : base(criteria)
    {
    }
}