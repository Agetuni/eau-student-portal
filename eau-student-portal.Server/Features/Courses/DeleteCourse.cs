using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Courses;

public class DeleteCourseCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteCourseHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Set<Course>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
        {
            return Result.Failure("Course not found.");
        }

        _context.Set<Course>().Remove(course);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

