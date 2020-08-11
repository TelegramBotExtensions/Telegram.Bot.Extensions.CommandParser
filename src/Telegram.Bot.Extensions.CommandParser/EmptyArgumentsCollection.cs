using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Telegram.Bot.Extensions.CommandParser
{
    internal class EmptyArgumentsCollection : IArgumentsCollection
    {
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count { get; } = 0;
        public bool ContainsKey(string key) => false;
        public bool TryGetValue(string key, [NotNullWhen(true)] out string value)
        {
            value = null!;
            return false;
        }

        public string this[string key] => throw new KeyNotFoundException($"Key '{key}' not found");
        public IEnumerable<string> Keys { get; } = new string[0];
        public IEnumerable<string> Values { get; } = new string[0];
        public TValue Get<TValue>(string key, string? format = default) =>
            throw new KeyNotFoundException($"Key '{key}' not found");
    }
}
