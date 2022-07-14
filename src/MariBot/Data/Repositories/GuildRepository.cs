using MariBot.Data.Contexts;
using MariBot.Data.Models;

namespace MariBot.Data.Repositories;

public class GuildRepository : BaseRepository<GuildModel>
{
    public GuildRepository(DataContext context, UnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
