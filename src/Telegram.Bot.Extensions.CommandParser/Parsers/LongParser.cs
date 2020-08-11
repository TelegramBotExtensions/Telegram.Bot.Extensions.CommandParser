using System;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class LongParser : IArgumentParser
    {
        public Type Type => typeof(long);
        public object? Parse(string value, string? format = default)
        {
            if (long.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
