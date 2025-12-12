using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Users;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public int Id { get; set; }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly ApplicationDbContext _context;

    public GetUserByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return Result<UserDto>.Failure("User not found.");
        }

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

