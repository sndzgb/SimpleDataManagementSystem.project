using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Backend.Database;
using SimpleDataManagementSystem.Backend.Logic.Options;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;

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
                var emailService = services.GetRequiredService<IEmailService>();

                var emailClientOptions = services.GetRequiredService<IOptions<EmailClientOptions>>();
                var appOptions = services.GetRequiredService<IOptions<AppOptions>>();

                DbInitializer.Initialize(context, emailService, appOptions, emailClientOptions);
            }
            catch (Exception ex)
            {
                // check why init failed; only required on startup/ init
            }

            return app;
        }
    }
}
