using Microsoft.Data.Sqlite;
using SimpleDataManagementSystem.Backend.WebAPI.Options;

namespace SimpleDataManagementSystem.Backend.WebAPI.DbInit
{
    public class SqliteDbInitializer
    {
        // Add table: Notifications content: Body (JSON), Receiver, IsRead, IsSent, /DateTime/, Sender, 
        internal static void Initialize(ConfigurationManager configuration)
        {
            // TODO clear table on startup; delete all items from table(s) -- WHERE 1 = 1
            const string CONNECTIONS_TABLE_NAME = "connections";
            var sqliteOptions = configuration.GetSection(nameof(SqliteOptions)).Get<SqliteOptions>();

            ArgumentNullException.ThrowIfNull(sqliteOptions, nameof(sqliteOptions));

            using (SqliteConnection con = new SqliteConnection(sqliteOptions.GetConnectionString()))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $@"
CREATE TABLE IF NOT EXISTS {CONNECTIONS_TABLE_NAME}
(
    userId INTEGER NOT NULL,
    connectionId TEXT NOT NULL,
    hubId INTEGER NOT NULL,
    PRIMARY KEY (userId, connectionId, hubId)
) 
WITHOUT ROWID
";

                command.ExecuteScalar();
            }
        }
    }
}
