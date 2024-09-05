using SimpleDataManagementSystem.Backend.Database;

namespace SimpleDataManagementSystem.Backend.WebAPI.DbInit
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder UseSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<SimpleDataManagementSystemDbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                // check why init failed; only required on startup/ init
            }

            return app;
        }
    }
}
