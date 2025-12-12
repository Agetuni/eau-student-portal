using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eau_student_portal.Server.Features.Courses;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetAllCourses(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCoursesQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCourseByIdQuery { Id = id }, cancellationToken);
        
        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetCourseById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] UpdateCourseCommand command, CancellationToken cancellationToken)
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
    public async Task<ActionResult> DeleteCourse(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCourseCommand { Id = id }, cancellationToken);
        
        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }

        return NoContent();
    }
}

