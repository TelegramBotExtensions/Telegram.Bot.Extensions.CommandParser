using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Telegram.Bot.Extensions.CommandParser
{
    public class CommandParser
    {
        private const string CommandTokenPattern =
            @"\{\{(?<variable>[a-zA-Z0-9_]+?)(:(?<type>[a-z]+?))?\}\}";

        private const string VariableTokenPattern = @"(?<{0}>{1})";

        private readonly Regex _regex;

        public CommandParser(CommandParserOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            var matchCollection = Regex.Matches(
                Options.CommandFormat,
                CommandTokenPattern,
                RegexOptions.IgnoreCase
            );

            var variableList = new List<Variable>();
            foreach (Match match in matchCollection)
            {
                var variable = CreateVariable(match);

                if (variableList.Contains(variable))
                {
                    throw new InvalidOperationException(
                        $"Variable name '{match}' is used more than once"
                    );
                }

                variableList.Add(variable);
            }

            Variables = variableList;

            var format = Options.CommandFormat;
            foreach (var variable in Variables)
            {
                var replacement = string.Format(
                    VariableTokenPattern,
                    variable.Name,
                    variable.Type.Pattern
                );
                format = format.Replace(
                    variable.OriginalPattern,
                    replacement
                );
            }

            _regex = new Regex($"^{format}$", RegexOptions.IgnoreCase);
        }

        public CommandParserOptions Options { get; }
        public IReadOnlyList<Variable> Variables { get; }

        /// <summary>
        /// Extract variable values from a given instance of the command you're trying to parse.
        /// </summary>
        /// <param name="command">The command instance.</param>
        /// <returns>
        /// An instance of <see cref="CommandParserResult"/> indicating success or failure with a
        /// dictionary of Variable names mapped to values if success.
        /// </returns>
        public CommandParserResult ParseCommand(string command)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));

            var inputValues = new Dictionary<string, string>();
            var matchCollection = _regex.Match(command);

            if (matchCollection.Success)
            {
                foreach (var variable in Variables)
                {
                    var value = matchCollection.Groups[variable.Name].Value;
                    inputValues.Add(variable.Name, value);
                }

                var arguments = new ArgumentsCollection(Options.Parsers, inputValues);
                return CommandParserResult.Success(arguments, Options.CommandFormat);
            }

            return CommandParserResult.Failure(Options.CommandFormat);
        }

        private Variable CreateVariable(Match match)
        {
            if (!match.Groups["variable"].Success)
                throw new InvalidOperationException("Command format is invalid");

            var variableType = Options.DefaultVariableType;

            var variableTypeName = match.Groups["type"].Success
                ? match.Groups["type"].Value
                : null;

            if (!string.IsNullOrEmpty(variableTypeName) &&
                !Options.VariableTypes.TryGetValue(variableTypeName!, out variableType))
            {
                throw new InvalidOperationException($"Invalid variable type '{variableTypeName}'");
            }

            var variableName = match.Groups["variable"].Value;

            return new Variable(variableName, match.Value, variableType);
        }
    }
}
