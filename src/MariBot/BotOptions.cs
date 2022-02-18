using Microsoft.Extensions.Options;

namespace MariBot;

public class BotOptions : IOptions<BotOptions>
{
    public const string Bot = "Bot";

    BotOptions IOptions<BotOptions>.Value => this;

    public string Token { get; set; } = null!;
}
