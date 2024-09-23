using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Interfaces.SpecificationBuilders;

public interface ITransactionSpecificationBuilder : IBuilder<ISpecification<Transaction>>
{
    ITransactionSpecificationBuilder ApplyBook(Guid bookId);
    ITransactionSpecificationBuilder ApplyDataTimeRange(DateTime? from, DateTime? to);
    ITransactionSpecificationBuilder ApplyPaging(int skip, int take);
}