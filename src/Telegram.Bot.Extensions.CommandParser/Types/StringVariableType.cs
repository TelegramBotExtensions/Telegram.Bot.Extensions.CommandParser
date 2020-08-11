namespace Telegram.Bot.Extensions.CommandParser.Types
{
    public class StringVariableType : IVariableType
    {
        public string Pattern => "[a-zA-Z0-9_-]+";
        public string Name => "string";
    }
}
