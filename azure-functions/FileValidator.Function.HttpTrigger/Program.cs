using FileValidator.Domain.Contracts;
using FileValidator.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IFileValidatorService, FileValidatorService>();
    })
    .ConfigureFunctionsWebApplication()
    .Build();

host.Run();