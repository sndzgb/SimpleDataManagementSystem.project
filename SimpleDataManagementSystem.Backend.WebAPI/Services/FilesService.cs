using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services
{
    // TODO IFilesService; demo purposes only
    public static class FilesService
    {
        public static void Upload(string relativePath, Stream stream)
        {
            var assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            using (var fs = new FileStream(Path.Combine(assDirPath, relativePath), FileMode.CreateNew))
            {
                stream.CopyToAsync(fs).GetAwaiter().GetResult();
            }
        }

        public static void Delete(string relativePath)
        {
            var assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            File.Delete(Path.Combine(assDirPath, relativePath)); // TODO error handling; + log error & continue
        }
    }
}
