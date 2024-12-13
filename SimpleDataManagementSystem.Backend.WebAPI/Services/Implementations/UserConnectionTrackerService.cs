using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services.Implementations
{
    public class UserConnectionTrackerService : IUserConnectionTrackerService
    {
        private readonly IOptionsMonitor<SqliteOptions> _sqliteOptionsMonitor;
        private readonly ILogger<UserConnectionTrackerService> _logger;


        public UserConnectionTrackerService(
                IOptionsMonitor<SqliteOptions> sqliteOptionsMonitor,
                ILogger<UserConnectionTrackerService> logger
            )
        {
            _sqliteOptionsMonitor = sqliteOptionsMonitor;
            _logger = logger;
        }


        public void Add(Connection connection)
        {
            var commandText = $@"
BEGIN TRANSACTION;

    DELETE FROM connections WHERE ((userId = @userId) AND (hubId = @hubId)); -- remove any old connection

    INSERT INTO connections (userId, connectionId, hubId) VALUES (@userId, @connectionId, @hubId);

COMMIT;
";

            try
            {
                using (var sqliteConnection = new SqliteConnection(_sqliteOptionsMonitor.CurrentValue.GetConnectionString()))
                {
                    sqliteConnection.Open();

                    using (var command = new SqliteCommand(commandText, sqliteConnection))
                    {
                        command.Parameters.Add(new SqliteParameter("@userId", connection.UserId));
                        command.Parameters.Add(new SqliteParameter("@connectionId", connection.ConnectionId));
                        command.Parameters.Add(new SqliteParameter("@hubId", (int)connection.Hub));

                        var rowInserted = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, ex.Message);
                // TODO new exception
                throw new Exception("Error adding user to connections table. Try again by refreshing the page.");
            }
        }


        #region Get

        // TODO get all connections; remove PK composite; multiple tabs open?
        public Connection Get(int userId, HubService hubService)
        {
            string connectionId = string.Empty;

            var commandText = $@"
BEGIN TRANSACTION;

    SELECT connectionId FROM connections WHERE ((userId = @userId) AND (hubId = @hubId));

COMMIT;
";

            try
            {
                using (var sqliteConnection = new SqliteConnection(_sqliteOptionsMonitor.CurrentValue.GetConnectionString()))
                {
                    sqliteConnection.Open();

                    using (var command = new SqliteCommand(commandText, sqliteConnection))
                    {
                        command.Parameters.Add(new SqliteParameter("@userId", userId));
                        command.Parameters.Add(new SqliteParameter("@hubId", (int)hubService));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                connectionId = reader.GetString(0);
                            }
                        }
                    }
                }

                return new Connection()
                {
                    ConnectionId = connectionId,
                    Hub = hubService,
                    UserId = userId
                };
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, ex.Message);
                // TODO new exception
                throw new Exception("Error finding user in connections table.");
            }
        }

        #endregion


        public void Remove(Connection connection)
        {
            var commandText = $@"
BEGIN TRANSACTION;

    DELETE FROM connections WHERE ((userId = @userId) AND (hubId = @hubId)); -- remove if there is any old connection lingering

COMMIT;
";

            try
            {
                using (var sqliteConnection = new SqliteConnection(_sqliteOptionsMonitor.CurrentValue.GetConnectionString()))
                {
                    sqliteConnection.Open();

                    using (var command = new SqliteCommand(commandText, sqliteConnection))
                    {
                        command.Parameters.Add(new SqliteParameter("@userId", connection.UserId));
                        command.Parameters.Add(new SqliteParameter("@hubId", (int)connection.Hub));

                        var rowInserted = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, ex.Message);
                // TODO new exception
                throw new Exception("Error removing user from connections table.");
            }
        }
    }
}
