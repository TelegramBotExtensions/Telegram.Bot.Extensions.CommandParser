using System;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class DoubleParser : IArgumentParser
    {
        public Type Type => typeof(double);
        public object? Parse(string value, string? format = default)
        {
            if (double.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
