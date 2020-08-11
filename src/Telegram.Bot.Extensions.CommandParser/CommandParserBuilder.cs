using System;
using System.Collections.Generic;
using Telegram.Bot.Extensions.CommandParser.Parsers;
using Telegram.Bot.Extensions.CommandParser.Types;

namespace Telegram.Bot.Extensions.CommandParser
{
    public class CommandParserBuilder
    {
        private readonly Dictionary<Type, IArgumentParser> _argumentParsers =
            new Dictionary<Type, IArgumentParser>();

        private readonly Dictionary<string, IVariableType> _variableTypes =
            new Dictionary<string, IVariableType>();

        private string? _variableStartChar;
        private string? _variableEndChar;
        private string? _commandPattern;
        private IVariableType? _defaultVariableType;

        public CommandParserBuilder UseDefaultVariableType<TVariableType>()
            where TVariableType : IVariableType, new()
        {
            var variableType = Activator.CreateInstance<TVariableType>();
            return UseDefaultVariableType(variableType);
        }

        public CommandParserBuilder UseDefaultVariableType(IVariableType variableType)
        {
            _defaultVariableType = variableType;
            return this;
        }

        public CommandParserBuilder UseDefaultVariableType(string name)
        {
            if (!_variableTypes.ContainsKey(name))
                throw new ArgumentOutOfRangeException(
                    nameof(name),
                    name,
                    $"No variable type named '{name}'"
                );

            _defaultVariableType = _variableTypes[name];
            return this;
        }

        public CommandParserBuilder UseVariableType<TVariableType>()
            where TVariableType : IVariableType, new()
        {
            var variableType = Activator.CreateInstance<TVariableType>();
            return UseVariableType(variableType);
        }

        public CommandParserBuilder UseArgumentParser<TArgumentParser>()
            where TArgumentParser : IArgumentParser, new()
        {
            var argumentParser = Activator.CreateInstance<TArgumentParser>();
            return UseArgumentParser(argumentParser);
        }

        public CommandParserBuilder UseArgumentParser(IArgumentParser parser)
        {
            if (parser is null) throw new ArgumentNullException(nameof(parser));

            _argumentParsers[parser.Type] = parser;

            return this;
        }

        public CommandParserBuilder UseVariableType(IVariableType variableType)
        {
            if (variableType is null) throw new ArgumentNullException(nameof(variableType));

            _variableTypes[variableType.Name] = variableType;

            return this;
        }

        public CommandParserBuilder UseVariableDelimiters(string start, string end)
        {
            _variableStartChar = start;
            _variableEndChar = end;

            return this;
        }

        public CommandParserBuilder UseCommandPattern(string commandPattern)
        {
            if (string.IsNullOrWhiteSpace(commandPattern))
                throw new ArgumentNullException(nameof(commandPattern));

            _commandPattern = commandPattern;

            return this;
        }

        public CommandParser Build()
        {
            if (string.IsNullOrWhiteSpace(_commandPattern))
                throw new InvalidOperationException("Command pattern is not set");

            if (string.IsNullOrWhiteSpace(_variableStartChar))
                throw new InvalidOperationException("Start variable character is not set");

            if (string.IsNullOrWhiteSpace(_variableStartChar))
                throw new InvalidOperationException("End variable character is not set");

            if (_variableTypes.Count == 0)
                throw new InvalidOperationException("Variable types is empty");

            if (_argumentParsers.Count == 0)
                throw new InvalidOperationException("Argument parsers is empty");

            if (_defaultVariableType is null)
                throw new InvalidOperationException("Default variable type is not set");

            if (_variableStartChar is null)
                throw new InvalidOperationException("Start char can't be null");

            if (_variableEndChar is null)
                throw new InvalidOperationException("End char can't be null");

            var variableTypes = new Dictionary<string, IVariableType>();

            foreach (var variableType in _variableTypes)
            {
                variableTypes[variableType.Key] = variableType.Value ??
                    throw new InvalidOperationException(
                        $"'{variableType}' variable type is not set"
                    );
            }

            var argumentParsers = new Dictionary<Type, IArgumentParser>();
            foreach (var argumentParser in _argumentParsers)
            {
                argumentParsers[argumentParser.Key] =
                    argumentParser.Value ?? throw new InvalidOperationException(
                        $"'Parser for type {argumentParser.Key.Name}' is not set"
                    );
            }

            var options = new CommandParserOptions(
                _commandPattern!,
                variableTypes,
                argumentParsers,
                _defaultVariableType,
                _variableStartChar,
                _variableEndChar
            );

            return new CommandParser(options);
        }

        public static CommandParserBuilder CreateDefaultBuilder() =>
            new CommandParserBuilder()
                .UseVariableType(new StringVariableType())
                .UseVariableType(new IntVariableType())
                .UseVariableType(new LongVariableType())
                .UseVariableType(new DoubleVariableType())
                .UseVariableType(new BoolVariableType())
                .UseArgumentParser(new StringParser())
                .UseArgumentParser(new IntParser())
                .UseArgumentParser(new LongParser())
                .UseArgumentParser(new DoubleParser())
                .UseArgumentParser(new BoolParser())
                .UseDefaultVariableType("string")
                .UseVariableDelimiters("{{", "}}");

        public static CommandParser CreateDefaultParser(string commandPattern) =>
            new CommandParserBuilder()
                .UseVariableType(new StringVariableType())
                .UseVariableType(new IntVariableType())
                .UseVariableType(new LongVariableType())
                .UseVariableType(new DoubleVariableType())
                .UseVariableType(new BoolVariableType())
                .UseArgumentParser(new StringParser())
                .UseArgumentParser(new IntParser())
                .UseArgumentParser(new LongParser())
                .UseArgumentParser(new DoubleParser())
                .UseArgumentParser(new BoolParser())
                .UseDefaultVariableType(new StringVariableType())
                .UseVariableDelimiters("{{", "}}")
                .UseCommandPattern(commandPattern)
                .Build();
    }
}
