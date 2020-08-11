using System;

namespace Telegram.Bot.Extensions.CommandParser
{
    public sealed class Variable : IEquatable<Variable>
    {
        public string Name { get; }
        public string OriginalPattern { get; }
        public IVariableType Type { get; }

        public Variable(string name, string originalPattern, IVariableType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OriginalPattern = originalPattern ??
                              throw new ArgumentNullException(nameof(originalPattern));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public bool Equals(Variable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Variable) obj);
        }

        public override int GetHashCode() =>
            unchecked((Name != null ? Name.GetHashCode() : 0) * 397);
    }
}
