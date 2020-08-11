using System;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class StringParser : IArgumentParser
    {
        public Type Type => typeof(string);
        public object? Parse(string value, string? format = default) => value;
    }
}
