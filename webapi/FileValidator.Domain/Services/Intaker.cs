using DataProcessor;
using DataProcessor.DataSource.InStream;
using DataProcessor.InputDefinitionFile;
using DataProcessor.InputDefinitionFile.Models;
using DataProcessor.Models;
using DataProcessor.ProcessorDefinition;
using FileValidator.Domain.Models;

namespace FileValidator.Domain.Services;

public static class Intaker
{
    public static ParsedDataAndSpec10 LoadVersion10(Stream data, string specsXml)
    {
        var inputDefinitionFile = FileLoader.LoadFromXml<InputDefinitionFile10>(specsXml);
        var fileProcessorDefinition = FileProcessorDefinitionBuilder.CreateFileProcessorDefinition(inputDefinitionFile);
        var result = new ParsedDataAndSpec10 { InputDefinitionFile = inputDefinitionFile };

        var config = new StreamDataSourceConfig
        {
            Delimiter = inputDefinitionFile.Delimiter,
            HasFieldsEnclosedInQuotes = inputDefinitionFile.HasFieldsEnclosedInQuotes,
            CommentedOutIndicator = inputDefinitionFile.CommentedOutIndicator
        };

        data.Seek(0, SeekOrigin.Begin);
        var source = new StreamDataSource<ParserContext10>(config, data);
        var processor = new ParsedDataProcessor10(source, fileProcessorDefinition);
        result.ParsedData = processor.Process();

        return result;
    }

    public static ParsedDataAndSpec20 LoadVersion20(Stream data, string specsXml)
    {
        var inputDefinitionFile = FileLoader.LoadFromXml<InputDefinitionFile20>(specsXml);
        var fileProcessorDefinition = FileProcessorDefinitionBuilder.CreateFileProcessorDefinition(inputDefinitionFile);
        var result = new ParsedDataAndSpec20 { InputDefinitionFile = inputDefinitionFile };

        var config = new StreamDataSourceConfig
        {
            Delimiter = inputDefinitionFile.Delimiter,
            HasFieldsEnclosedInQuotes = inputDefinitionFile.HasFieldsEnclosedInQuotes,
            CommentedOutIndicator = inputDefinitionFile.CommentedOutIndicator
        };

        data.Seek(0, SeekOrigin.Begin);
        var source = new StreamDataSource<ParserContext20>(config, data);
        var processor = new ParsedDataProcessor20(source, fileProcessorDefinition);
        result.ParsedData = processor.Process();

        return result;
    }

    public static string GetSpecsVersion(string specsXml)
    {
        var inputDefinitionFileVersion = HelperXmlSerializer.Deserialize<InputDefinitionFrameworkVersion>(specsXml);
        return inputDefinitionFileVersion.FrameworkVersion;
    }
}
