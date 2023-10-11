// See https://aka.ms/new-console-template for more information

using FileValidator.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
    });


var host = builder.Build();
var app = ActivatorUtilities.CreateInstance<ConsoleApplication>(host.Services);

void ConsoleCancelHandler(object? sender, ConsoleCancelEventArgs args)
{
    app.Terminate();
    args.Cancel = true;
}

Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCancelHandler);
app.Run();
