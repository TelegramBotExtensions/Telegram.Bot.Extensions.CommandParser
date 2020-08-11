namespace Telegram.Bot.Extensions.CommandParser
{
    public interface IVariableType
    {
        string Pattern { get; }
        string Name { get; }
    }
}
