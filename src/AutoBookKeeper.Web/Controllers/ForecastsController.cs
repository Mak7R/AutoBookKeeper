using System.ComponentModel.DataAnnotations;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class ForecastsController : ApiController
{
    private readonly IForecastService _forecastService;

    public ForecastsController(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/forecasts/polynomial-balance")]
    public async Task<IActionResult> PolynomialBalance(Guid bookId, [Required, FromQuery] DateTime endDate, [FromQuery] int daysStep = 1)
    {
        return Ok(await _forecastService.PolynomialBalanceForecast(bookId, endDate.Date, daysStep));
    }
}