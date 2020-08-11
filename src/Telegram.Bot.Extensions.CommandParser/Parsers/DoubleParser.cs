using System;
using System.Globalization;

namespace Telegram.Bot.Extensions.CommandParser.Parsers
{
    public class DoubleParser : IArgumentParser
    {
        private static readonly NumberStyles Style = NumberStyles.AllowDecimalPoint |
                                                     NumberStyles.AllowLeadingSign;

        public Type Type => typeof(double);
        public object? Parse(string value, string? format = default)
        {
            if (double.TryParse(value, Style, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
