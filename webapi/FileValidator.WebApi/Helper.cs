using System.Reflection;

namespace FileValidator.WebApi
{
    public static class Helper
    {
        public static string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version!.ToString();
    }
}
