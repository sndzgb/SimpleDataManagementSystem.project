using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using System.Diagnostics;

namespace SimpleDataManagementSystem.Backend.WebAPI.Hubs
{
    [AllowAnonymous]
    //[Authorize]
    public class ItemUpdatedNotifierHub : Hub<IItemUpdatedNotifierHub>
    {
        private readonly IOptionsMonitor<SqliteOptions> _sqliteOptionsMonitor;
        private readonly IUserConnectionTrackerService _userConnectionTrackerService;
        
        private const HubService HUB = HubService.ItemUpdatedNotifier;


        public ItemUpdatedNotifierHub(
                IOptionsMonitor<SqliteOptions> sqliteOptionsMonitor,
                IUserConnectionTrackerService userConnectionTrackerService
            )
        {
            _sqliteOptionsMonitor = sqliteOptionsMonitor;
            _userConnectionTrackerService = userConnectionTrackerService;
        }


        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var items = httpContext?.Items;
            string? accessToken = (string?)items["AccessToken"];

            //string? accessToken = await Context?.GetHttpContext()?.GetTokenAsync("access_token");

            // reject if null?
            var user = Context.User;

            if (user == null)
            {
                return;
            }

            _userConnectionTrackerService.Add(new Connection()
            {
                UserId = Convert.ToInt32(Context.User?.Identity?.Name),
                ConnectionId = Context.ConnectionId,
                Hub = HUB
            });

            //await AddUserIfNotExists(Context.User?.Identity?.Name, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _userConnectionTrackerService.Remove(new Connection()
            {
                UserId = Convert.ToInt32(Context.User?.Identity?.Name),
                ConnectionId = Context.ConnectionId,
                Hub = HUB
            });

            await base.OnDisconnectedAsync(exception);
        }


        #region helpers

        private async Task AddUserIfNotExists(string? name, string connectionId)
        {
            //var connString = _configuration.GetConnectionString("HubUserStateManagementDbConnectionString");


            //try
            //{
            //    var cs = _sqliteOptionsMonitor.CurrentValue.HubUserStateManagementDbConnectionString;
            //}
            //catch (Exception e)
            //{
            //    throw;
            //}


            //var cb = new SqliteConnectionStringBuilder("")
            //{
            //    Mode = SqliteOpenMode.ReadWriteCreate
            //};

            //using (var connection = new SqliteConnection("Data Source=|Data Directory|\\HubManagementDb.db"))
            //using (var connection = new SqliteConnection("Data Source=D:\\repos\\Samples\\SignalR.Server.WebAPI\\bin\\Debug\\net7.0\\HubManagementDb.db"))
            //using (var connection = new SqliteConnection(_sqliteOptionsMonitor.CurrentValue.HubUserStateManagementDbConnectionString))
            var connString = _sqliteOptionsMonitor.CurrentValue.GetConnectionString();
            //using (var connection = new SqliteConnection("Data Source=D:\\repos\\Samples\\SignalR.Server.WebAPI\\bin\\Debug\\net7.0\\sqlitedatabase\\HubUserStateManagementDb.db"))
            using (var connection = new SqliteConnection(connString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    //                    var insertCommand = connection.CreateCommand();
                    //                    insertCommand.Transaction = transaction;
                    //                    insertCommand.CommandText = $@"
                    //CREATE TABLE messages (
                    //	text TEXT NOT NULL
                    //);
                    //INSERT INTO messages (text) VALUES ($text)";
                    //                    insertCommand.Parameters.AddWithValue("$text", "Hello, World!");
                    //                    insertCommand.ExecuteNonQuery();

                    var selectCommand = connection.CreateCommand();
                    selectCommand.Transaction = transaction;
                    selectCommand.CommandText = "SELECT text FROM messages";

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var message = reader.GetString(0);
                            Debug.WriteLine(message);
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        #endregion
    }
}
