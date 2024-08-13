using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Web.Controllers.Base;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class TransactionTypesController : ApiController
{
    private readonly ITransactionTypesService _transactionTypesService;
    private readonly IMapper _mapper;

    public TransactionTypesController(ITransactionTypesService transactionTypesService, IMapper mapper)
    {
        _transactionTypesService = transactionTypesService;
        _mapper = mapper;
    }
}