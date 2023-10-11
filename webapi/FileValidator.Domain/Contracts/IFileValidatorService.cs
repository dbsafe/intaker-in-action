namespace FileValidator.Domain.Contracts;

public interface IFileValidatorService
{
    IList<string> ValidateFiles(Stream dataFile, Stream specsFile);
}
