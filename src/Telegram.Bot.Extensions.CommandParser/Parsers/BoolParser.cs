using System;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class BoolParser : IArgumentParser
    {
        public Type Type => typeof(bool);
        public object? Parse(string value, string? format = default)
        {
            if (bool.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
