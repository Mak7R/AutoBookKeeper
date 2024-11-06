using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Extensions;
using AutoBookKeeper.Core.Interfaces.SpecificationBuilders;
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
    
    public static IBookSpecificationBuilder GetBuilder() => new BookSpecificationBuilder();
    
    private class BookSpecificationBuilder : IBookSpecificationBuilder
    {
        private Guid? _ownerId;

        public IBookSpecificationBuilder ApplyOwner(Guid ownerId)
        {
            _ownerId = ownerId;
            return this;
        }

        private string? _title;

        public IBookSpecificationBuilder ApplyTitle(string? title)
        {
            _title = title;
            return this;
        }

        private (int Skip, int Take)? _paging;

        public IBookSpecificationBuilder ApplyPaging(int skip, int take)
        {
            _paging = (skip, take);
            return this;
        }
        
        public ISpecification<Book> Build()
        {
            Expression<Func<Book, bool>> criteria = b => true;

            if (_ownerId.HasValue)
            {
                criteria = criteria.AndAlso(b => b.OwnerId == _ownerId.Value);
            }

            if (!string.IsNullOrEmpty(_title))
            {
                criteria = criteria.AndAlso(b => b.Title == _title);
            }
            
            var spec = new BookSpecification(criteria);
            
            if (_paging.HasValue)
            {
                spec.ApplyPaging(_paging.Value.Skip, _paging.Value.Take);
            }

            return spec;
        }
    }
}