using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Extensions;
using AutoBookKeeper.Core.Interfaces.SpecificationBuilders;
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

    public static ITransactionSpecificationBuilder GetBuilder() => new TransactionSpecificationBuilder();
    
    private class TransactionSpecificationBuilder : ITransactionSpecificationBuilder
    {
        private Guid? _bookId;

        public ITransactionSpecificationBuilder ApplyBook(Guid bookId)
        {
            _bookId = bookId;
            return this;
        }
        
        private (DateTime? From, DateTime? To)? _transactionDataTimeRange;

        public ITransactionSpecificationBuilder ApplyDataTimeRange(DateTime? from, DateTime? to)
        {
            _transactionDataTimeRange = (from, to);
            return this;
        }

        private (int Skip, int Take)? _paging;

        public ITransactionSpecificationBuilder ApplyPaging(int skip, int take)
        {
            _paging = (skip, take);
            return this;
        }
        
        public ISpecification<Transaction> Build()
        {
            Expression<Func<Transaction, bool>> criteria = t => true;

            if (_bookId.HasValue)
            {
                criteria = criteria.AndAlso(t => t.BookId == _bookId.Value);
            }

            if (_transactionDataTimeRange.HasValue)
            {
                var from = _transactionDataTimeRange.Value.From;
                var to = _transactionDataTimeRange.Value.To;

                if (from.HasValue && to.HasValue)
                {
                    criteria = criteria.AndAlso(t => t.TransactionTime >= from.Value && t.TransactionTime <= to.Value);
                }
                else if (from.HasValue)
                {
                    criteria = criteria.AndAlso(t => t.TransactionTime >= from.Value);
                }
                else if (to.HasValue)
                {
                    criteria = criteria.AndAlso(t => t.TransactionTime <= to.Value);
                }
            }
            
            var spec = new TransactionSpecification(criteria);
            
            if (_paging.HasValue)
            {
                spec.ApplyPaging(_paging.Value.Skip, _paging.Value.Take);
            }

            return spec;
        }
    }
}