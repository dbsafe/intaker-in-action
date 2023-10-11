using System.Text.Json.Serialization;

namespace FileValidator.Client
{
    internal class FileValidatorClientManagerConfig
    {
        public IEnumerable<FilePair>? FilePairs { get; set; }
        public int Count { get; set; }
        public TimeSpan Interval { get; set; }
        public string? Url { get; set; }
    }

    internal class FilePair
    {
        [JsonIgnore]
        public byte[] Data { get; }
        public string DataFileName { get; }
        [JsonIgnore]
        public byte[] Specs { get; }
        public string SpecsFileName { get; }

        public FilePair(byte[] data, string dataFileName, byte[] specs, string specsFileName)
        {
            Data = data;
            DataFileName = dataFileName;
            Specs = specs;
            SpecsFileName = specsFileName;
        }
    }
}
