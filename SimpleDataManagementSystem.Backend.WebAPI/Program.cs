using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SimpleDataManagementSystem.Backend.Database;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Database.Repositories.Implementations;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Options;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Implementations;
using SimpleDataManagementSystem.Backend.WebAPI.DbInit;
using SimpleDataManagementSystem.Backend.WebAPI.Filters;
using SimpleDataManagementSystem.Backend.WebAPI.Hubs;
using SimpleDataManagementSystem.Backend.WebAPI.Middlewares;
using SimpleDataManagementSystem.Backend.WebAPI.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Implementations;
using SimpleDataManagementSystem.Shared.Extensions;
using SimpleDataManagementSystem.Shared.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string HUBS_PREFIX = "/hubs";

            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;
            
            //builder.Logging.ClearProviders();

            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.User.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var connectionString = builder.Configuration.GetConnectionString("SimpleDataManagementSystemDbContext");

            var myCorsPolicy = "MyCorsPolicy";

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            builder.Services.AddOptions<CorsOptions>().BindConfiguration(nameof(CorsOptions));

            builder.Services.AddCors(options =>
            {
                var corsOptions = builder.Configuration.GetRequiredSection(CorsOptions.CorsOptionsSectionName).Get<CorsOptions>();
                ArgumentNullException.ThrowIfNull(corsOptions, nameof(corsOptions));
                options.AddPolicy(myCorsPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins(corsOptions.AllowedOrigins)
                            //.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithExposedHeaders("Set-Authorization");
                    });
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["JwtOptions:Issuer"],
                    ValidAudience = config["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtOptions:Key"]!) // TODO get key from secure location: key vault, secrets, ...
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                x.SaveToken = true;

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // ff the request is for our hub...
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments(HUBS_PREFIX)))
                        {
                            // ...read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                //options.DefaultPolicy = new AuthorizationPolicyBuilder()
                //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                //    .RequireAuthenticatedUser()
                //    .Build();

                options.AddDefaultPolicies();

                //x.AddPolicy("AdminOnly", p => p.RequireClaim("", ""));
                //options.AddPolicy(
                //    "UserIsResourceOwnerPolicy",
                //    policy => policy.Requirements.Add(new UserIsResourceOwnerAuthorizationRequirement())
                //);
                //options.AddPolicy(
                //    "UserIsInRolePolicy",
                //    policy => policy.Requirements.Add(new UserIsInRoleAuthorizationRequirement())
                //);
            }).AddServicesForDefaultPolicies();

            builder.Services.AddControllers(options =>
            {
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => 
                    $"The value '{x}' is not valid for {y.SplitCamelCase()}."
                );
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => 
                    $"The value for '{x.SplitCamelCase()}' is not valid."
                );

                options.Filters.Add(new ValidateModelActionFilter());
            }).AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options => 
                options.SuppressModelStateInvalidFilter = true
            );

            builder.Services.Configure<EmailClientOptions>
            (
                config.GetSection(EmailClientOptions.EmailOptionsSectionName)
            );
            
            builder.Services.Configure<AppOptions>
            (
                config.GetSection(AppOptions.AppOptionsSectionName)
            );

            builder.Services.AddOptions<SqliteOptions>().BindConfiguration(nameof(SqliteOptions));

            builder.Services.AddDbContextFactory<SimpleDataManagementSystemDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            builder.Services.AddDbContext<SimpleDataManagementSystemDbContext>(options =>
                options.UseSqlServer(connectionString)
            );


            //builder.Services.AddIdentity<UserEntity, RoleEntity>()
            //    .AddDefaultTokenProviders()
            //    .AddEntityFrameworkStores<SimpleDataManagementSystemDbContext>();

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true;
            //});

            builder.Services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);
            //builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<INotificationsService, NotificationsService>();
            builder.Services.AddTransient<IItemUpdatedNotifierService, ItemUpdatedNotifierService>();
            builder.Services.AddTransient<IUserConnectionTrackerService, UserConnectionTrackerService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IItemsCoreService, ItemsCoreService>();
            builder.Services.AddScoped<IItemsCoreRepository, ItemsCoreRepository>();
            builder.Services.AddScoped<IUsersCoreService, UsersCoreService>();
            builder.Services.AddScoped<IUsersCoreRepository, UsersCoreRepository>();
            builder.Services.AddScoped<IRetailersCoreService, RetailersCoreService>();
            builder.Services.AddScoped<IRetailersCoreRepository, RetailersCoreRepository>();
            builder.Services.AddScoped<ICategoriesCoreService, CategoriesCoreService>();
            builder.Services.AddScoped<ICategoriesCoreRepository, CategoriesCoreRepository>();
            builder.Services.AddScoped<IRolesCoreService, RolesCoreService>();
            builder.Services.AddScoped<IRolesCoreRepository, RolesCoreRepository>();
            builder.Services.AddScoped<ITokenGeneratorService, JwtGeneratorService>();

            builder.Services.Configure<JwtOptions>(config.GetSection(JwtOptions.Jwt));

            // Configure the HTTP request pipeline.
            var app = builder.Build();

            InitiateSqliteDb(builder.Configuration);

            app.UseCors(myCorsPolicy);

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<AccessTokenMiddleware>();
            app.UseMiddleware<TokenSlidingExpirationMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AllowOnlyAuthenticatedMiddleware>();
            app.UseMiddleware<PasswordChangeRequiredCheckMiddleware>();

            app.MapControllers();

            app.MapHub<ItemUpdatedNotifierHub>($"{HUBS_PREFIX}/itemUpdatedNotifier");

            app.UseSeedSqlServer();

            app.Run();
        }


        // Add table: Notifications content: Body (JSON), Receiver, IsRead, IsSent, /DateTime/, Sender, 
        private static void InitiateSqliteDb(ConfigurationManager configuration)
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