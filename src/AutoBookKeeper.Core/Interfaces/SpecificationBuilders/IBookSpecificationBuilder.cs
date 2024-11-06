using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Interfaces.SpecificationBuilders;

public interface IBookSpecificationBuilder : IBuilder<ISpecification<Book>>
{
    IBookSpecificationBuilder ApplyOwner(Guid ownerId);
    IBookSpecificationBuilder ApplyTitle(string? title);
    IBookSpecificationBuilder ApplyPaging(int skip, int take);
}