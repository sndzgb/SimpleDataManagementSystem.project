using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Options
{
    public class SqliteOptions
    {
        public const string SqliteOptionsSectionName = "SqliteOptions";

        public string HubUserStateManagementDbName { get; set; }

        public string GetConnectionString()
        {
            var path = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
                    HubUserStateManagementDbName
                );

            var db = string.Concat("Data Source=", path);

            return db;
        }
    }
}
