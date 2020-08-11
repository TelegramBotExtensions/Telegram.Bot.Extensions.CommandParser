using System;
using System.Collections;
using System.Collections.Generic;

namespace Telegram.Bot.Extensions.CommandParser
{
    internal class ArgumentsCollection : IArgumentsCollection
    {
        private readonly IReadOnlyDictionary<Type, IArgumentParser> _parsers;
        private readonly IReadOnlyDictionary<string, string> _arguments;

        public ArgumentsCollection(
            IReadOnlyDictionary<Type, IArgumentParser> parsers,
            IReadOnlyDictionary<string, string> arguments)
        {
            _parsers = parsers;
            _arguments = arguments;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
            _arguments.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            _arguments.GetEnumerator();

        public int Count => _arguments.Count;
        public bool ContainsKey(string key) =>
            _arguments.ContainsKey(key);

        public bool TryGetValue(string key, out string value) =>
            _arguments.TryGetValue(key, out value);

        public string this[string key] => _arguments[key];

        public IEnumerable<string> Keys => _arguments.Keys;
        public IEnumerable<string> Values => _arguments.Values;

        public TValue Get<TValue>(string key, string? format = default)
        {
            if (!_arguments.TryGetValue(key, out var valueString))
            {
                throw new KeyNotFoundException($"Key '{key}' not found");
            }

            if (!_parsers.TryGetValue(typeof(TValue), out var parser))
                throw new InvalidOperationException(
                    $"{typeof(TValue).Name} parser for '{key}' is not found"
                );

            object? result;

            try
            {
                result = parser.Parse(valueString, format);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Can't parse {typeof(TValue).Name} value for '{key}'",
                    exception
                );
            }

            if (!(result is TValue))
            {
                throw new InvalidOperationException(
                    $"Can't parse {typeof(TValue).Name} value for '{key}'"
                );
            }

            return (TValue) result;
        }
    }
}
