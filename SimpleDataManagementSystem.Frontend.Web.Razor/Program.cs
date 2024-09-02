using SimpleDataManagementSystem.Frontend.Web.Razor.Middlewares;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;

namespace SimpleDataManagementSystem.Frontend.Web.Razor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            
            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddTransient<IAccountsService, AccountsService>();
            builder.Services.AddTransient<IRetailersService, RetailersService>();
            builder.Services.AddTransient<ICategoriesService, CategoriesService>();
            builder.Services.AddTransient<IItemsService, ItemsService>();
            builder.Services.AddTransient<IRolesService, RolesService>();

            builder.Services.AddAuthentication().AddCookie("MyCookie", options =>
            {
                options.Cookie.Name = "MyCookie";

                //options.LoginPath = "/Account/Login";
                options.LoginPath = "/";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
            });

            builder.Services.AddHttpClient("SimpleDataManagementSystemHttpClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7006");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});

            app.Run();
        }
    }
}