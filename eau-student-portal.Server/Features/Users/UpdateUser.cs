using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Users;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly ApplicationDbContext _context;

    public UpdateUserHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return Result<UserDto>.Failure("User not found.");
        }

        // Check if email is being changed and if it already exists
        if (user.Email != request.Email)
        {
            var emailExists = await _context.Set<User>()
                .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

            if (emailExists)
            {
                return Result<UserDto>.Failure("A user with this email already exists.");
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        });
    }
}

