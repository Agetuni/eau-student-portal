using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Courses;

public class UpdateCourseCommand : IRequest<Result<CourseDto>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
}

public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Result<CourseDto>>
{
    private readonly ApplicationDbContext _context;

    public UpdateCourseHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CourseDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Set<Course>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
        {
            return Result<CourseDto>.Failure("Course not found.");
        }

        // Check if course code is being changed and if it already exists
        if (course.Code != request.Code)
        {
            var codeExists = await _context.Set<Course>()
                .AnyAsync(c => c.Code == request.Code && c.Id != request.Id, cancellationToken);

            if (codeExists)
            {
                return Result<CourseDto>.Failure("A course with this code already exists.");
            }
        }

        course.Name = request.Name;
        course.Code = request.Code;
        course.Description = request.Description;
        course.Credits = request.Credits;
        course.UpdatedAt = DateTime.UtcNow;

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

