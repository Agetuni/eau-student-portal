using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Courses;

public class CreateCourseCommand : IRequest<Result<CourseDto>>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
}

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<CourseDto>>
{
    private readonly ApplicationDbContext _context;

    public CreateCourseHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        // Check if course code already exists
        var codeExists = await _context.Set<Course>()
            .AnyAsync(c => c.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            return Result<CourseDto>.Failure("A course with this code already exists.");
        }

        var course = new Course
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Credits = request.Credits,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<Course>().Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<CourseDto>.Success(new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Code = course.Code,
            Description = course.Description,
            Credits = course.Credits,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        });
    }
}

