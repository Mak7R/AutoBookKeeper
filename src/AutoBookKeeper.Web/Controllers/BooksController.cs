using AutoBookKeeper.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class BooksController : ApiController
{
    public BooksController()
    {
        
    }

    [Authorize]
    [HttpGet("books")]
    public IActionResult GetAll()
    {
        throw new NotImplementedException();
    }
}