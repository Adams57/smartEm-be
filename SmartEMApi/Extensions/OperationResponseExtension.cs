using Microsoft.AspNetCore.Mvc;
using SmartEM.Application.Utils;

namespace SmartEMApi.Extensions;

public static class OperationResponseExtension
{
    public static ActionResult ResponseResult(this OperationResponse response)
    {
        return response.Code switch
        {
            StatusCode.BadRequest => new BadRequestObjectResult(response),
            StatusCode.NotFound => new NotFoundObjectResult(response),
            StatusCode.Unauthorized => new UnauthorizedObjectResult(response),
            StatusCode.InternalServerError => new InternalServerErrorObjectResult(response),
            StatusCode.OK => new OkObjectResult(response),
            _ => new OkObjectResult(response),
        };
    }


    public static ActionResult<T> ResponseResult<T>(this OperationResponse response)
    {
        return response.Code switch
        {
            StatusCode.BadRequest => new BadRequestObjectResult(response),
            StatusCode.NotFound => new NotFoundObjectResult(response),
            StatusCode.Unauthorized => new UnauthorizedObjectResult(response),
            StatusCode.InternalServerError => new InternalServerErrorObjectResult(response),
            StatusCode.OK => new OkObjectResult(response),
            _ => new OkObjectResult(response),
        };
    }
}
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value) : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}
