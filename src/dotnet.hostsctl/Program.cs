using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{

#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif

    config.AddCommand<ListCommand>("list");
    config.AddCommand<OpenCommand>("open");
    config.AddCommand<BackupCommand>("backup");
    config.AddCommand<RestoreCommand>("restore");
    config.AddCommand<AddCommand>("add");
    config.AddCommand<RemoveCommand>("remove");
    config.AddCommand<EnableCommand>("enable");
    config.AddCommand<DisableCommand>("disable");
    config.AddCommand<ExistsCommand>("exists");
});

return app.Run(args);