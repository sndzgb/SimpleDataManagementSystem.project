using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Logging;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.DelegatingHandlers;
using SimpleDataManagementSystem.Frontend.Web.Razor.Events;
using SimpleDataManagementSystem.Frontend.Web.Razor.Middlewares;
using SimpleDataManagementSystem.Frontend.Web.Razor.Options;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Shared.Extensions;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            IdentityModelEventSource.ShowPII = true;

            var myCorsPolicy = "MyCorsPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader()
                        .WithExposedHeaders("Set-Authentication");
                    });
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<AppendAuthorizationHeaderDelegatingHandler>();
            builder.Services.AddTransient<RefreshAuthorizationHeaderDelegatingHandler>();
            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddTransient<IAccountsService, AccountsService>();
            builder.Services.AddTransient<IRetailersService, RetailersService>();
            builder.Services.AddTransient<ICategoriesService, CategoriesService>();
            builder.Services.AddTransient<IItemsService, ItemsService>();
            builder.Services.AddTransient<IRolesService, RolesService>();

            builder.Services.AddScoped<CookieAuthEvents>();

            builder.Services.Configure<SimpleDataManagementSystemHttpClientOptions>
            (
                configuration.GetSection(SimpleDataManagementSystemHttpClientOptions.SimpleDataManagementSystemHttpClient)
            );

            builder.Services.AddAuthentication().AddCookie(Cookies.AuthenticationCookie.Name, options =>
            {
                options.Cookie.Name = Cookies.AuthenticationCookie.Name;
                options.Cookie.HttpOnly = true;
                
                //options.LoginPath = "/Account/Login";
                options.LoginPath = "/";
                options.AccessDeniedPath = "/Forbidden";

                //options.SlidingExpiration = true;
                //options.Cookie.Expiration = TimeSpan.FromMinutes(1440);
                options.Cookie.MaxAge = TimeSpan.FromMinutes(1440);

                options.EventsType = typeof(CookieAuthEvents);

                //options.Events.OnRedirectToAccessDenied = c =>
                //{
                //    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //    return Task.FromResult<object>(null);
                //};
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddDefaultPolicies();

                //options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                //options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                //options.AddPolicy("RequireResourceOwnership", policy => policy.RequireClaim("UserId", "{UserId}"));
            }).AddServicesForDefaultPolicies();

            builder.Services.AddHttpClient(HttpClients.SimpleDataManagementSystemHttpClient.Name, client =>
            {
                var httpClientOptions = configuration.GetSection
                (
                    SimpleDataManagementSystemHttpClientOptions.SimpleDataManagementSystemHttpClient
                )
                .Get<SimpleDataManagementSystemHttpClientOptions>();

                if (httpClientOptions == null)
                {
                    throw new NullReferenceException(nameof(httpClientOptions));
                }

                client.BaseAddress = new Uri(httpClientOptions.Url);
            })
            .AddHttpMessageHandler<AppendAuthorizationHeaderDelegatingHandler>()
            .AddHttpMessageHandler<RefreshAuthorizationHeaderDelegatingHandler>();

            var app = builder.Build();

            app.UseCors(myCorsPolicy);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) // TODO change to production
            {
                // TODO
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Error/500"); // global error from web api call

                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            //handler=PageError
            app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<AllowPasswordChangeMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}