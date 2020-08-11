namespace Telegram.Bot.Extensions.CommandParser
{
    public class CommandParserResult
    {
        public IArgumentsCollection Variables { get; }
        public bool Successful { get; }
        public string CommandFormat { get; }

        private CommandParserResult(string commandFormat, IArgumentsCollection? variables = default)
        {
            CommandFormat = commandFormat;
            Variables = variables ?? new EmptyArgumentsCollection();
            Successful = variables != null;
        }

        internal static CommandParserResult Failure(string commandFormat)
            => new CommandParserResult(commandFormat);

        internal static CommandParserResult Success(
            IArgumentsCollection values,
            string commandFormat)
            => new CommandParserResult(commandFormat, values);
    }
}
