namespace MariBot.Data.Models;

public class GuildModel : BaseModel
{
    public GuildModel(ulong id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; set; }
}
