namespace LakeRun.Bot.Commands;

public class CommandsManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _commands;

    public CommandsManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _commands = new Dictionary<string, Type>
        {
            { "/start", typeof(StartCommand) }
        };
    }

    public IBotCommand GetCommand(string commandName)
    {
        if (_commands.TryGetValue(commandName, out Type commandType))
        {
            return _serviceProvider.GetRequiredService(commandType) as IBotCommand ?? throw new InvalidOperationException();
        }
        
        return null;
    }
}