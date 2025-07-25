using Application.Users.Commands;
using Application.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Web.Api.Extensions;
using Web.Api.Requests.User;

using static Web.Api.Extensions.FinExtensions;

namespace Web.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateUserRequest request, CancellationToken token)
    {
        var command = new CreateUserCommand(request.Firstname, request.Lastname, request.Email);

        return (await sender.Send(command, token)).ToActionResult(guid => CreatedAtAction(nameof(Get), new { id = guid.ToString() }, guid.ToString()),
        error => GetFailActionResult(error, HttpContext.Request.Path.ToString()));

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewModel>> Get([FromRoute] string id, CancellationToken token)
    {
        var command = new GetUserByIdQuery(id);

        return (await sender.Send(command, token))
        .ToActionResult(u => Ok(u), error => GetFailActionResult(error, HttpContext.Request.Path.ToString()));

    }
}