using System.ComponentModel.DataAnnotations.Schema;

namespace MariBot.Data.Models;

public abstract class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public virtual ulong Id { get; set; }

    public virtual DateTime CreatedAt { get; set; }

    public virtual DateTime UpdatedAt { get; set; }
}
