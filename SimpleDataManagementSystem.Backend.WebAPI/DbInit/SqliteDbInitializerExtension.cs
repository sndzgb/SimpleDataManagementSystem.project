using Serilog;

namespace SimpleDataManagementSystem.Backend.WebAPI.DbInit
{
    internal static class SqliteDbInitializerExtension
    {
        public static IApplicationBuilder UseInitializeSqliteDb(
                this IApplicationBuilder applicationBuilder, 
                ConfigurationManager configurationManager
            )
        {
            ArgumentNullException.ThrowIfNull(applicationBuilder, nameof(applicationBuilder));
            
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                SqliteDbInitializer.Initialize(configurationManager);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, string.Empty);
                throw;
            }

            return applicationBuilder;
        }
    }
}
