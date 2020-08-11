namespace Telegram.Bot.Extensions.CommandParser.Types
{
    public class LongVariableType : IVariableType
    {
        public string Pattern => "([0]{1}|-?[1-9]{1}[0-9]{0,18})";
        public string Name => "long";
    }
}
