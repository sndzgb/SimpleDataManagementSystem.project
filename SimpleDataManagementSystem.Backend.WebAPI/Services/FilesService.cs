using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services
{
    // TODO IFilesService; demo purposes only
    public static class FilesService
    {
        public static void Upload(string relativePath, Stream stream)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return;
            }

            var assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var completePath = Path.Combine(assDirPath, relativePath);

            if (File.Exists(completePath))
            {
                return;
            }

            // Path.Combine(assDirPath, relativePath)
            using (var fs = new FileStream(completePath, FileMode.CreateNew))
            {
                stream.CopyToAsync(fs).GetAwaiter().GetResult();
            }
        }

        public static void Delete(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) 
            {
                return;
            }

            var assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var completePath = Path.Combine(assDirPath, relativePath);

            if (!File.Exists(completePath))
            {
                return;
            }

            File.Delete(completePath); // TODO error handling; + log error & continue -- eg.: file in use, ...
        }
    }
}
