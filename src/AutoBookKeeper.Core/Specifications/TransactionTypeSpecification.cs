using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Specifications;

public class TransactionTypeSpecification : BaseSpecification<TransactionType>
{
    public TransactionTypeSpecification()
    {
    }
    
    public TransactionTypeSpecification(Expression<Func<TransactionType, bool>> criteria) : base(criteria)
    {
    }   
}