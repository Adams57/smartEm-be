using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartEMApi.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class BaseController : Controller
{
    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }
    protected IMediator Mediator { get; }
}
