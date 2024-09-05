using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using SimpleDataManagementSystem.Backend.Database;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Database.Repositories.Implementations;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Implementations;
using SimpleDataManagementSystem.Backend.WebAPI.DbInit;
using SimpleDataManagementSystem.Backend.WebAPI.Filters;
using SimpleDataManagementSystem.Backend.WebAPI.Middlewares;
using SimpleDataManagementSystem.Backend.WebAPI.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Implementations;
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
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            var connectionString = builder.Configuration.GetConnectionString("SimpleDataManagementSystemDbContext");

            var myCorsPolicy = "MyCorsPolicy";

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // TODO test
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicy,
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
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
                        Encoding.UTF8.GetBytes(config["JwtOptions:Key"]!) // TODO get key from secure location: valut, ...
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //builder.Services.AddAuthorization(x =>
            //{
            //    x.AddPolicy("AdminOnly", p => p.RequireClaim("", ""));
            //});

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelActionFilter());
            }).AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options => 
                options.SuppressModelStateInvalidFilter = true
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


            builder.Services.AddScoped<IItemsService, ItemsService>();
            builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IRetailersService, RetailersService>();
            builder.Services.AddScoped<IRetailersRepository, RetailersRepository>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IRolesRepository, RolesRepository>();
            builder.Services.AddScoped<ITokenGeneratorService, JwtGeneratorService>();

            builder.Services.Configure<JwtOptions>(config.GetSection(JwtOptions.Jwt));
            //builder.Services.Configure<JwtOptions>(config.GetSection("JwtOptions"));

            // Configure the HTTP request pipeline.
            var app = builder.Build();

            app.UseCors(myCorsPolicy);

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<TokenSlidingExpirationMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSeedSqlServer();
            }

            app.Run();
        }
    }
}