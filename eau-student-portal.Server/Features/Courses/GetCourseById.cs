using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Courses;

public class GetCourseByIdQuery : IRequest<Result<CourseDto>>
{
    public int Id { get; set; }
}

public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCourseByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Set<Course>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
        {
            return Result<CourseDto>.Failure("Course not found.");
        }

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

