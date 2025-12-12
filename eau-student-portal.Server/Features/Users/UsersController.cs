using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eau_student_portal.Server.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
        
        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand { Id = id }, cancellationToken);
        
        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }

        return NoContent();
    }
}

