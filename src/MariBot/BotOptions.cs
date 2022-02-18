using Microsoft.Extensions.Options;

namespace MariBot;

public class BotOptions : IOptions<BotOptions>
{
    BotOptions IOptions<BotOptions>.Value => this;

    public string Token { get; set; } = null!;
}
