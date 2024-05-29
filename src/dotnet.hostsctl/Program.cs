using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("dotnet hostsctl");
    config.SetApplicationVersion("1.0.0");

#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif

    config.AddCommand<ListCommand>("list")
        .WithDescription("Prints hosts entries")
        .WithExample("list", "-i", "../samples/hosts");

    config.AddCommand<BackupCommand>("backup")
        .WithDescription("Backup hosts file")
        .WithExample("backup", "-i", "../samples/hosts", "-o", "../samples/hosts.bak");

    config.AddCommand<RestoreCommand>("restore")
        .WithDescription("Restore hosts file from backup")
        .WithExample("restore", "-i", "../samples/hosts.bak", "-o", "../samples/hosts");

    config.AddCommand<AddCommand>("add")
        .WithDescription("Adds entry to hosts file")
        .WithExample("add", "app.mydomain.local")
        .WithExample("add", "app.mydomain.local", "::1");
        //.WithExample("add", "app.mydomain.local", "-i", "../samples/hosts", "-o", "../samples/hosts.bak");

    config.AddCommand<RemoveCommand>("remove")
        .WithDescription("Removes entry from hosts file");

    config.AddCommand<EnableCommand>("enable")
        .WithDescription("Enables entry in hosts file");

    config.AddCommand<DisableCommand>("disable")
        .WithDescription("Disables entry in hosts file");

    config.AddCommand<ExistsCommand>("exists")
        .WithDescription("Checks if entry exists in hosts file");

    if (OperatingSystem.IsWindows())
    {
        config.AddCommand<OpenCommand>("open")
        .WithDescription("Opens hosts file (using shell execute)");
    }
});

return app.Run(args);