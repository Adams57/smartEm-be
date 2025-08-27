using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartEM.Application.Features.Users.Commands.CreateUser;
using SmartEM.Application.Features.Users.Commands.DeleteUser;
using SmartEM.Application.Features.Users.Commands.UpdateUser;
using SmartEM.Application.Features.Users.Queries.FetchUser;
using SmartEMApi.Extensions;

namespace SmartEMApi.Controllers;

public class UserController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand request)
    {
        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
    {
        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUsers(DeleteUserCommand request)
    {
        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FetchUserById([FromRoute] Guid id)
    {
        var request = new FetchUserByIdQuery
        {
            Id = id
        };
        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }

    [HttpGet]
    public async Task<IActionResult> FetchUsers([FromQuery] FetchUsersQuery request)
    {

        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }
}
