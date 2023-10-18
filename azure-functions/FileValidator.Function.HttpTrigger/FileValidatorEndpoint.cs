using FileValidator.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FileValidator.Function.HttpTrigger
{
    public class FileValidatorEndpoint
    {
        private readonly ILogger _logger;
        private readonly IFileValidatorService _fileValidatorService;

        public FileValidatorEndpoint(ILoggerFactory loggerFactory, IFileValidatorService fileValidatorService)
        {
            _logger = loggerFactory.CreateLogger<FileValidatorEndpoint>();
            _fileValidatorService = fileValidatorService;
        }

        [Function("file-validator/validate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var formData = await req.ReadFormAsync();
            var files = formData.Files.ToList();

            if (!TryExtractFiles(files, out Stream? data, out Stream? specs, out string error))
            {
                _logger.LogError(error);
                var modelStates = new ModelStateDictionary();
                modelStates.AddModelError("Files", error);
                return new BadRequestObjectResult(modelStates);
            }

            return new OkObjectResult(_fileValidatorService.ValidateFiles(data!, specs!));
        }

        private static bool TryExtractFiles(List<IFormFile> files, out Stream? data, out Stream? specs, out string error)
        {
            data = null;
            specs = null;
            error = string.Empty;

            if (files.Count != 2)
            {
                error = $"Invalid number of files. Two files are expected {files.Count} was received.";
                return false;
            }

            IFormFile? dataFile = null;
            IFormFile? specFile = null;
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                switch (ext)
                {
                    case ".csv":
                        dataFile = file;
                        break;
                    case ".xml":
                        specFile = file;
                        break;
                }
            }

            if (dataFile is null || specFile is null)
            {
                error = "Invalid file type. Two files are expected, one '.csv' and one '.xml'.";
                return false;
            }

            data = dataFile.OpenReadStream();
            specs = specFile.OpenReadStream();

            return true;
        }
    }
}
