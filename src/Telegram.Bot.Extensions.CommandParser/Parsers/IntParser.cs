using System;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class IntParser : IArgumentParser
    {
        public Type Type => typeof(int);
        public object? Parse(string value, string? format = default)
        {
            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
