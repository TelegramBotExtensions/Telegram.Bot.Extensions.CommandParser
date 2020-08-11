namespace Telegram.Bot.Extensions.CommandParser.Types
{
    public class BoolVariableType : IVariableType
    {
        public string Pattern => "(true|false)";
        public string Name => "bool";
    }
}
