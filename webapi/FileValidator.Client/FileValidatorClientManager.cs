using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FileValidator.Client
{
    internal class FileValidatorClientManager
    {
        private readonly ILogger<FileValidatorClientManager> _logger;
        private readonly FileValidatorClientManagerConfig _config;
        private readonly ILoggerFactory _loggerFactory;
        private readonly Task[] _tasks;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        public FileValidatorClientManager(ILoggerFactory loggerFactory, FileValidatorClientManagerConfig config)
        {
            _loggerFactory = loggerFactory;
            _config = config;

            _logger = _loggerFactory.CreateLogger<FileValidatorClientManager>();
            LogConfiguration();

            if (_config.FilePairs is null || !_config.FilePairs.Any())
            {
                throw new InvalidOperationException($"{nameof(_config.FilePairs)} cannot be empty");
            }

            _tasks = CreateTasks();
        }

        public void Run()
        {
            _logger.LogInformation("Starting clients...");
            foreach (var task in _tasks)
            {
                task.Start();
            }

            _logger.LogInformation("Clients started.");
            Task.WaitAll(_tasks);
        }

        public void Terminate()
        {
            _logger.LogInformation("Stopping clients...");
            _cancellationTokenSource.Cancel();
            Task.WaitAll(_tasks);
            _logger.LogInformation("Clients stopped.");
        }

        private void LogConfiguration()
        {
            var json = JsonSerializer.Serialize(_config, _jsonSerializerOptions);
            _logger.LogInformation("Configuration:{NewLine}{json}", Environment.NewLine, json);
        }

        private Task[] CreateTasks()
        {
            _logger.LogInformation("Starting clients...");
            var filePairs = _config.FilePairs!.ToArray();
            var filePairIndex = 0;

            var tasks = new Task[_config.Count];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(CreateClientAction(i, filePairs[filePairIndex], _cancellationTokenSource.Token));
                filePairIndex++;
                if (filePairIndex == filePairs.Length)
                {
                    filePairIndex = 0;
                }
            }

            return tasks;
        }

        private Action CreateClientAction(int index, FilePair filePair, CancellationToken token)
        {
            return () =>
            {
                var fileValidatorClientLogger = _loggerFactory.CreateLogger($"{typeof(FileValidatorClient)}_{index}");
                var client = new FileValidatorClient(fileValidatorClientLogger, _config.Url!);

                fileValidatorClientLogger.LogInformation("FilePair:{NewLine}{json}", Environment.NewLine, JsonSerializer.Serialize(filePair, _jsonSerializerOptions));

                while (!token.IsCancellationRequested)
                {
                    // client.GetVersion();
                    client.ValidateFile(filePair);
                    Thread.Sleep(_config.Interval);
                }
            };
        }
    }
}
