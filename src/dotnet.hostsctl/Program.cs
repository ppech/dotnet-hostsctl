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
});

return app.Run(args);