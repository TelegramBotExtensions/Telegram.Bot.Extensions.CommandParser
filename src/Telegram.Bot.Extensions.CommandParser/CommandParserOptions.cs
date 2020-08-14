using System;
using System.Collections.Generic;

namespace Telegram.Bot.Extensions.CommandParser
{
    public class CommandParserOptions
    {
        public string CommandFormat { get; }
        public IVariableType DefaultVariableType { get; }
        public IReadOnlyDictionary<string, IVariableType> VariableTypes { get; }
        public IReadOnlyDictionary<Type, IArgumentParser> Parsers { get; }

        public CommandParserOptions(
            string commandFormat,
            IReadOnlyDictionary<string, IVariableType> variableTypes,
            IReadOnlyDictionary<Type, IArgumentParser> parsers,
            IVariableType defaultVariableType)
        {
            if (string.IsNullOrWhiteSpace(commandFormat))
                throw new ArgumentNullException(nameof(commandFormat));

            CommandFormat = commandFormat;
            VariableTypes = variableTypes;
            Parsers = parsers;
            DefaultVariableType = defaultVariableType;
        }
    }
}
