using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Users;

public class DeleteUserCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteUserHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return Result.Failure("User not found.");
        }

        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

