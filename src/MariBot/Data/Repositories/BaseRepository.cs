using MariBot.Data.Contexts;
using MariBot.Data.Models;
using MariBot.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace MariBot.Data.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : BaseModel
{
    protected readonly DataContext _context;
    protected readonly UnitOfWork _unitOfWork;

    protected BaseRepository(DataContext context, UnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public IQueryable<TEntity> Query()
    {
        return _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query().ToArrayAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(ulong id, CancellationToken cancellationToken = default)
    {
        return await Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _ = await _context.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (!_unitOfWork.TransactionOpened)
        {
            _ = await _unitOfWork.CommitAsync(true, cancellationToken);
        }

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _ = _context.Update(entity);

        if (!_unitOfWork.TransactionOpened)
        {
            _ = await _unitOfWork.CommitAsync(true, cancellationToken);
        }

        return entity;
    }
}
