using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MariBot.Data.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        _ = builder.ToTable(GetTableName());
    }

    protected virtual string GetTableName()
    {
        var pluralizedName = typeof(TEntity).Name.Replace("Model", string.Empty).Pluralize();

        return string.Concat(pluralizedName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString().ToLower()));
    }
}
