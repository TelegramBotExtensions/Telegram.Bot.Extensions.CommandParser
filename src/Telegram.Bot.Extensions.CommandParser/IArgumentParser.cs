using System;

namespace Telegram.Bot.Extensions.CommandParser
{
    public interface IArgumentParser
    {
        Type Type { get; }
        object? Parse(string value, string? format = default);
    }
}
