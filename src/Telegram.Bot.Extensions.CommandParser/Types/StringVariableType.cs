namespace Telegram.Bot.Extensions.CommandParser.Types
{
    public class StringVariableType : IVariableType
    {
        public string Pattern => "\\S+";
        public string Name => "string";
    }
}
