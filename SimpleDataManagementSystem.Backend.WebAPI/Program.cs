using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
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
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("SimpleDataManagementSystemDbContext");

            //Seed(null);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelActionFilter());
            });

            builder.Services.Configure<ApiBehaviorOptions>(options => 
                options.SuppressModelStateInvalidFilter = true
            );

            builder.Services.AddDbContext<SimpleDataManagementSystemDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            builder.Services.AddScoped<IItemsService, ItemsService>();
            builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
            builder.Services.AddScoped<IRetailersService, RetailersService>();
            builder.Services.AddScoped<IRetailersRepository, RetailersRepository>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IRolesRepository, RolesRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //EnsureDbExists(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseItToSeedSqlServer();
            }

            app.Run();
        }

        #region db seed

        private static void EnsureDbExists(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<SimpleDataManagementSystemDbContext>();

                var newlyCreated = dbContext.Database.EnsureCreated();

                //SeedDb(dbContext);
                //if (newlyCreated)
                //{
                //}
            }
        }

        private static void SeedDb(SimpleDataManagementSystemDbContext dbContext = null)
        {
            try
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var x = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                var xml = Path.Combine(x, "Resources");


                string strPath = Path.Combine(xml, "retailers.xml");


                //XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "Retailers";
                //xRoot.IsNullable = true;

                //var serializer = new XmlSerializer(typeof(Retailer), xRoot);
                //using (var reader = new StreamReader(strPath))
                //{
                //    var person = (Retailer)serializer.Deserialize(reader);
                //    //Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
                //}


                //var categoriesDb = GetCategories();
                //var retailersDb = GetRetailers();
                //var itemsDb = GetItems();

                //dbContext.Categories.Add(categoriesDb);


                XmlDocument doc = new XmlDocument();
                doc.Load(strPath);
                XmlElement root = doc.DocumentElement;

                XmlNodeList retailers = doc.SelectNodes("/Retailers/Retailer");

                List<RetailerEntity> rets = new List<RetailerEntity>();
                foreach (XmlNode r in retailers)
                {
                    int id = Convert.ToInt32(r.SelectSingleNode("ID").InnerText);
                    string name = r.SelectSingleNode("Name").InnerText;
                    int priority = Convert.ToInt32(r.SelectSingleNode("Priority").InnerText);
                    string logoImageUrl = r.SelectSingleNode("LogoImageUrl").InnerText;

                    var model = new RetailerEntity();
                    model.ID = id;
                    model.Name = name;
                    model.Priority = priority;
                    model.LogoImageUrl = logoImageUrl;

                    rets.Add(model);




                    using (HttpClient httpClient = new HttpClient())
                    {
                        //var d = Directory.GetCurrentDirectory();
                        var d = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        var di = Directory.CreateDirectory(Path.Combine(d, @"Images\Retailers"));

                        //var resourcesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                        //var resourcesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));


                        //var temp = @"d:\temp";
                        var temp = di.FullName;
                        Directory.CreateDirectory(temp);

                        var ext = Path.GetExtension(model.LogoImageUrl);
                        var uri = new Uri(model.LogoImageUrl);
                        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                        using (var fs = new FileStream(Path.Combine(temp, model.Name + ext), FileMode.CreateNew))
                        {
                            response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                        }
                    }






                    // TODO single instance
                    //using (HttpClient client = new HttpClient())
                    //{
                    //    var temp = @"d:\temp";
                    //    Directory.CreateDirectory(temp);
                    //    client.BaseAddress = new Uri(model.LogoImageUrl);
                    //    var imageBytes = client.GetByteArrayAsync(strPath).GetAwaiter().GetResult();
                    //    File.WriteAllBytesAsync(temp, imageBytes).GetAwaiter().GetResult();

                    //    //client.DownloadFile(new Uri(model.LogoImageUrl), @$"d:\temp\{model.Name}.png");
                    //    // OR 
                    //    //client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
                    //}


                    //XmlNode idNode = r.SelectSingleNode("ID");
                    //if (idNode != null)
                    //{
                    //    string idString = r.SelectSingleNode("ID").InnerText;
                    //}
                }

                dbContext.Retailers.AddRange(rets);
                dbContext.SaveChanges();

                //XmlNode node = doc.DocumentElement.SelectSingleNode(@"/Retailers/Retailer");
                //XmlNode childNode = node.ChildNodes[0];
                //if (childNode is XmlCDataSection)
                //{
                //    XmlCDataSection cdataSection = childNode as XmlCDataSection;
                //}
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}