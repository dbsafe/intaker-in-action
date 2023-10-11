using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace FileValidator.Client
{
    internal class FileValidatorClient
    {
        private static readonly HttpClient _httpClient = new();       
        private readonly ILogger _logger;
        private readonly string _url;

        public FileValidatorClient(ILogger logger, string url)
        {
            _logger = logger;
            _url = url;
        }

        public void ValidateFile(FilePair filePair)
        {
            try
            {
                using var form = new MultipartFormDataContent();

                using var dataContent = new ByteArrayContent(filePair.Data);
                dataContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
                form.Add(dataContent, "files", Path.GetFileName(filePair.DataFileName));

                using var specsContent = new ByteArrayContent(filePair.Specs);
                specsContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml");
                form.Add(specsContent, "files", Path.GetFileName(filePair.SpecsFileName));

                var response = _httpClient.PostAsync($"{_url}/api/FileValidator/validate", form).Result;
                var body = response.Content.ReadAsStringAsync().Result;
                _logger.LogInformation("Response Body:{NewLine}{body}", Environment.NewLine, body);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }
        }
    }
}
