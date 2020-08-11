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
        public string VariableStartChar { get; }
        public string VariableEndChar { get; }

        public CommandParserOptions(
            string commandFormat,
            IReadOnlyDictionary<string, IVariableType> variableTypes,
            IReadOnlyDictionary<Type, IArgumentParser> parsers,
            IVariableType defaultVariableType,
            string variableStartChar,
            string variableEndChar)
        {
            if (string.IsNullOrWhiteSpace(commandFormat))
                throw new ArgumentNullException(nameof(commandFormat));

            CommandFormat = commandFormat;
            VariableTypes = variableTypes;
            Parsers = parsers;
            DefaultVariableType = defaultVariableType;
            VariableStartChar = variableStartChar ??
                                throw new ArgumentNullException(nameof(variableStartChar));
            VariableEndChar = variableEndChar ??
                              throw new ArgumentNullException(nameof(variableEndChar));
        }
    }
}
