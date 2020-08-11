namespace Telegram.Bot.Extensions.CommandParser.Types
{
    public class IntVariableType : IVariableType
    {
        public string Pattern => "([0]{1}|-?[1-9]{1}[0-9]{0,9})";
        public string Name => "int";
    }
}
