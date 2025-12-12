using eau_student_portal.Server.Infrastructure.Data;
using eau_student_portal.Server.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eau_student_portal.Server.Features.Users;

public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
{
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetAllUsersHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Set<User>()
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<List<UserDto>>.Success(users);
    }
}

