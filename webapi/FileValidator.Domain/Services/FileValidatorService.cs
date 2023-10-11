using DataProcessor.InputDefinitionFile;
using FileValidator.Domain.Contracts;

namespace FileValidator.Domain.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        public IList<string> ValidateFiles(Stream dataFile, Stream specsFile)
        {
            var specsXml = ReadXml(specsFile);
            var specsVersion = Intaker.GetSpecsVersion(specsXml);
            
            return specsVersion switch
            {
                InputDefinitionFile10.VERSION => Intaker.LoadVersion10(dataFile, specsXml).ParsedData!.Errors,
                InputDefinitionFile20.VERSION => Intaker.LoadVersion20(dataFile, specsXml).ParsedData!.Errors,
                _ => throw new ArgumentException($"Invalid file specification version '{specsVersion}'"),
            };
        }

        private static string ReadXml(Stream specs)
        {
            specs.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(specs);
            return reader.ReadToEnd();
        }
    }
}
