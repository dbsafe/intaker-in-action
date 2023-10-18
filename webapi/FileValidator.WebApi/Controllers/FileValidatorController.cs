using FileValidator.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FileValidator.WebApi.Controllers;

[Route("api/file-validator")]
[ApiController]
public class FileValidatorController : ControllerBase
{
    private readonly IFileValidatorService _fileValidatorService;

    public FileValidatorController(IFileValidatorService fileValidatorService)
    {
        _fileValidatorService = fileValidatorService;
    }

    [HttpPost("validate")]
    public IActionResult ValidateFile(List<IFormFile> files)
    {
        if (!TryExtractFiles(files, out Stream? data, out Stream? specs, out string error))
        {
            ModelState.AddModelError("Files", error);
            return UnprocessableEntity(ModelState);
        }

        return Ok(_fileValidatorService.ValidateFiles(data!, specs!));
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
