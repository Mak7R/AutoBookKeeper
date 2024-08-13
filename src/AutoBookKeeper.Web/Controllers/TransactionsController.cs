using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Web.Controllers.Base;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class TransactionsController : ApiController
{
    private readonly ITransactionsService _transactionsService;
    private readonly IMapper _mapper;

    public TransactionsController(ITransactionsService transactionsService, IMapper mapper)
    {
        _transactionsService = transactionsService;
        _mapper = mapper;
    }
}