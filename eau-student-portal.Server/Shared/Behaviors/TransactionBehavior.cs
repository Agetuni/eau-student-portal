using eau_student_portal.Server.Infrastructure.Data;
using MediatR;

namespace eau_student_portal.Server.Shared.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ApplicationDbContext _context;

    public TransactionBehavior(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only apply transaction for commands (write operations)
        // Queries don't need transactions
        if (request is IRequest<TResponse> && typeof(TRequest).Name.Contains("Command"))
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        return await next();
    }
}

