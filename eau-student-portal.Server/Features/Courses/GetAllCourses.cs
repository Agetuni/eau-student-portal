using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Courses;

public class GetAllCoursesQuery : IRequest<Result<List<CourseDto>>>
{
}

public class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, Result<List<CourseDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetAllCoursesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _context.Set<Course>()
            .OrderBy(c => c.Code)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                Credits = c.Credits,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<List<CourseDto>>.Success(courses);
    }
}

