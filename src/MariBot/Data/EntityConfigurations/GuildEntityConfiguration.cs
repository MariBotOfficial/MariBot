using MariBot.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MariBot.Data.EntityConfigurations;

public class GuildEntityConfiguration : BaseEntityConfiguration<GuildModel>
{
    public override void Configure(EntityTypeBuilder<GuildModel> builder)
    {
        _ = builder.HasKey(x => x.Id);
        _ = builder.Property(x => x.Id);

        base.Configure(builder);
    }
}
