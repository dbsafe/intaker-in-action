using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FileValidator.Client;

internal class ConsoleApplication
{
    private readonly FileValidatorClientManager _clientManager;

    private static readonly string? _currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    //private const string URL = "https://localhost:7157"; // local web api
    //private const string URL = "https://localhost:7207"; // local azure function
    //private const string URL = "https://web-app-20231017.azurewebsites.net"; // web api in azure
    private const string URL = "https://func-file-validator-20231018.azurewebsites.net"; // function in azure
    
    private const int COUNT = 2;
    private static readonly TimeSpan _interval = TimeSpan.FromSeconds(1);
    private static readonly FilePair[] _filePairs = new FilePair[]
    {
        BuildFilePair("multi-data-type-with-errors.v20.csv", "Balance20.xml"),
        BuildFilePair("multi-data-type.v20.csv", "Balance20.xml"),
    };

    private static FilePair BuildFilePair(string dataFileName, string specsFileName)
    {
        dataFileName = BuildFullPath(dataFileName);
        specsFileName = BuildFullPath(specsFileName);

        EnsureFileExists(dataFileName);
        EnsureFileExists(specsFileName);

        var data = File.ReadAllBytes(dataFileName);
        var specs = File.ReadAllBytes(specsFileName);
        return new FilePair(data, dataFileName, specs, specsFileName);
    }

    private static void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File [{path}] not found.");
        }
    }

    private static string BuildFullPath(string fileName)
    {
        return Path.Combine(_currentDirectory!, "sample-files", fileName);
    }

    private static readonly FileValidatorClientManagerConfig _clientManagerConfig = new()
    {
        Url = URL,
        FilePairs = _filePairs,
        Count = COUNT,
        Interval = _interval,
    };

    public ConsoleApplication(ILoggerFactory loggerFactory)
    {
        _clientManager = new FileValidatorClientManager(loggerFactory, _clientManagerConfig);
    }

    // This is a good example of how to setup a batch console application
    // but for a process than can be started and stopped using a service project is more convenient so we don't have to implement the stopping mechanism.
    public void Run() => _clientManager.Run();

    public void Terminate() => _clientManager.Terminate();
}
