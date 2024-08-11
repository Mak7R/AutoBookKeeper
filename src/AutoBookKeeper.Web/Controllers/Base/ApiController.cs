using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers.Base;

[ApiController]
[Route("api/v{apiVersion}")]
public class ApiController : ControllerBase
{
}