using MariBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MariBot.Data.Services;

public class UnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public bool TransactionOpened { get; private set; }

    public void OpenTransaction()
    {
        TransactionOpened = true;
    }

    public void CloseTransaction()
    {
        TransactionOpened = false;
    }

    public async Task<int> CommitAsync(bool closeTransaction = true, CancellationToken cancellationToken = default)
    {
        var createdEntries = _context.ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Added);

        foreach (var entry in createdEntries)
        {
            entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }

        var updatedEntries = _context.ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Modified);

        foreach (var entry in updatedEntries)
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }

        var recordsChanged = await _context.SaveChangesAsync(cancellationToken);

        if (TransactionOpened && closeTransaction)
        {
            TransactionOpened = false;
        }

        return recordsChanged;
    }

    public Task RollBackAsync(bool closeTransaction = true, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var changedEntries = _context.ChangeTracker
            .Entries()
            .Where(x => x.State != EntityState.Unchanged);

        foreach (var entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    break;
            }
        }

        if (TransactionOpened && closeTransaction)
        {
            TransactionOpened = false;
        }

        return Task.CompletedTask;
    }
}
