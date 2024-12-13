using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Backend.Database;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.Options;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Implementations;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Reflection;
using System.Xml;

namespace SimpleDataManagementSystem.Backend.WebAPI.DbInit
{
    internal class DbInitializer
    {
        internal static void Initialize(
                SimpleDataManagementSystemDbContext dbContext,
                IEmailService emailService,
                IOptions<AppOptions> appOptions,
                IOptions<EmailClientOptions> emailClientOptions,
                ILogger<DbInitializer> logger
            )
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

            bool isMigrationNeeded = (dbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult()).Any();

            logger.LogInformation("isMigrationNeeded: {isMigrationNeeded}", isMigrationNeeded);

            if (isMigrationNeeded)
            {
                dbContext.Database.Migrate();

                // required
                SeedIfNeededRoles(dbContext);
                SeedIfNeededAdmin(dbContext, emailService, appOptions, emailClientOptions);

#if DEVELOPMENT
                // optional
                SeedIfNeededRetailers(dbContext);
                SeedIfNeededCategories(dbContext);
                SeedIfNeededItems(dbContext);
#endif
            }
        }


        private static void SeedIfNeededRoles(SimpleDataManagementSystemDbContext dbContext)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var roles = new List<RoleEntity>();
                var rolesEnum = Enum.GetValues(typeof(Roles)).Cast<Roles>();

                foreach (Roles role in rolesEnum)
                {
                    roles.Add(new RoleEntity()
                    {
                        Id = (int)role,
                        Name = role.ToString()
                    });
                }

                // TODO add logic to remove Role(s)
                var existingRoles = new HashSet<string>(dbContext.Roles.Select(p => p.Name));
                var newRoles = roles.Where(p => !existingRoles.Contains(p.Name)).ToList();

                if (newRoles.Count == 0)
                {
                    return;
                }

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Roles] ON");

                dbContext.Roles.AddRange(newRoles);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Roles] OFF");
                transaction.Commit();
            }
        }

        private static void SeedIfNeededAdmin(
                SimpleDataManagementSystemDbContext dbContext, 
                IEmailService emailService, 
                IOptions<AppOptions> appOptions, 
                IOptions<EmailClientOptions> emailClientOptions
            )
        {
            var user = new UserEntity();
            user.Username = "admin";
            user.Id = 1;
            user.RoleId = (int)Roles.Admin;
            
            var existingAdmin = dbContext.Users.Find(user.Id);
            
            if (existingAdmin != null) 
            {
                return;
            }

            var generatedPassword = GenerateRandomDefaultPassword();

            var hasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = hasher.HashPassword(user, generatedPassword);

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] ON");

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] OFF");

                transaction.Commit();
            }

            emailService.Send(new Email()
            {
                Body = $@"
<div style='text-align:center;'>
    <div>
        <p>Username:</p>
        <b>
            <label>{user.Username}</label>
        </b>
    </div>
    <div>
        <p>Password:</p>
        <b>
            <label>{generatedPassword}</label>
        </b>
    </div>
</div>
",
                Subject = "Admin account credentials",
                From = emailClientOptions.Value.NoReplyEmail,
                To = appOptions.Value.AdminEmail
            });
        }

        private static void SeedIfNeededRetailers(SimpleDataManagementSystemDbContext dbContext)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var assDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var xml = Path.Combine(assDir, "Resources");
                string retailersXmlPath = Path.Combine(xml, "retailers.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(retailersXmlPath);
                XmlElement root = doc.DocumentElement;
                XmlNodeList rets = doc.SelectNodes("/Retailers/Retailer");
                List<RetailerEntity> lRetailers = new List<RetailerEntity>();

                foreach (XmlNode r in rets)
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

                    using (HttpClient httpClient = new HttpClient())
                    {
                        var d = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        var di = Directory.CreateDirectory(Path.Combine(d, @"Images\Retailers"));

                        var temp = di.FullName;
                        Directory.CreateDirectory(temp);

                        var ext = Path.GetExtension(model.LogoImageUrl);
                        var uri = new Uri(model.LogoImageUrl);
                        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            model.LogoImageUrl = null;
                        }
                        else
                        {
                            using (var fs = new FileStream(Path.Combine(temp, model.Name + ext), FileMode.Create))
                            {
                                response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                            }

                            model.LogoImageUrl = Path.Combine("Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
                        }
                    }

                    lRetailers.Add(model);
                }

                var existingRetailers = new HashSet<int>(dbContext.Retailers.Select(p => p.ID)); 
                
                // include/ add any navigation properties if exist
                var newRetailers = lRetailers.Where(p => !existingRetailers.Contains(p.ID)).ToList();

                if (newRetailers == null || newRetailers.Count == 0)
                {
                    return;
                }

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Retailers] ON");

                dbContext.Retailers.AddRange(newRetailers);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Retailers] OFF");

                transaction.Commit();
            }
        }

        private static void SeedIfNeededCategories(SimpleDataManagementSystemDbContext dbContext)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var assDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var xml = Path.Combine(assDir, "Resources");
                string categoriesXmlPath = Path.Combine(xml, "categories.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(categoriesXmlPath);
                XmlElement root = doc.DocumentElement;
                XmlNodeList rets = doc.SelectNodes("/Categories/Category");
                List<CategoryEntity> lCategories = new List<CategoryEntity>();

                foreach (XmlNode r in rets)
                {
                    int id = Convert.ToInt32(r.SelectSingleNode("ID").InnerText);
                    string name = r.SelectSingleNode("Name").InnerText;
                    int priority = Convert.ToInt32(r.SelectSingleNode("Priority").InnerText);

                    var model = new CategoryEntity();
                    model.ID = id;
                    model.Name = name;
                    model.Priority = priority;

                    lCategories.Add(model);
                }

                var existingCategories = new HashSet<int>(dbContext.Categories.Select(p => p.ID));

                // include/ add any navigation properties if exist
                var newCategories = lCategories.Where(p => !existingCategories.Contains(p.ID)).ToList();

                if (newCategories == null || newCategories.Count == 0)
                {
                    return;
                }

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] ON");

                dbContext.Categories.AddRange(newCategories);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] OFF");

                transaction.Commit();
            }
        }

        private static void SeedIfNeededItems(SimpleDataManagementSystemDbContext dbContext)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var assDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var xml = Path.Combine(assDir, "Resources");
                string retailersXmlPath = Path.Combine(xml, "items.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(retailersXmlPath);
                XmlElement root = doc.DocumentElement;
                XmlNodeList items = doc.SelectNodes("/Items/Item");
                List<ItemEntity> lItems = new List<ItemEntity>();

                foreach (XmlNode r in items)
                {
                    int categoryId = Convert.ToInt32(r.SelectSingleNode("Kategorija").InnerText);
                    decimal price = Convert.ToDecimal(r.SelectSingleNode("Cijena").InnerText);
                    string discountDateRange = r.SelectSingleNode("Datumakcije").InnerText;
                    string productName = r.SelectSingleNode("Nazivproizvoda").InnerText;
                    string retailerName = r.SelectSingleNode("Nazivretailera").InnerText;
                    string description = r.SelectSingleNode("Opis").InnerText;
                    string urldoslike = r.SelectSingleNode("URLdoslike").InnerText;

                    var retailerEntity = dbContext.Retailers.Where(x => x.Name == retailerName).FirstOrDefault();

                    var model = new ItemEntity()
                    {
                        Kategorija = categoryId,
                        Cijena = price,
                        Datumakcije = discountDateRange,
                        Nazivproizvoda = productName,
                        Nazivretailera = retailerName,
                        Opis = description,
                        URLdoslike = urldoslike,
                        RetailerID = retailerEntity.ID
                    };

                    using (HttpClient httpClient = new HttpClient())
                    {
                        var d = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        var di = Directory.CreateDirectory(Path.Combine(d, @"Images\Items"));

                        var temp = di.FullName;
                        Directory.CreateDirectory(temp);

                        var ext = Path.GetExtension(model.URLdoslike);
                        var uri = new Uri(model.URLdoslike);

                        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                        if (response.IsSuccessStatusCode)
                        {
                            using (var fs = new FileStream(Path.Combine(temp, Path.GetFileName(uri.LocalPath)), FileMode.Create)) // TODO check file extension...
                            {
                                response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                            }

                            model.URLdoslike = Path.Combine("Images\\Items\\" + Path.GetFileName(model.URLdoslike));
                        }
                        else
                        {
                            model.URLdoslike = null;
                        }
                    }

                    lItems.Add(model);
                }

                // TODO redo this part
                var existingItems = new HashSet<string>(dbContext.Items.Select(p => p.Nazivproizvoda).AsQueryable());

                // include/ add any navigation properties if exist
                var newItems = lItems.Where(p => !existingItems.Contains(p.Nazivproizvoda)).ToList();

                if (newItems == null || newItems.Count == 0)
                {
                    return;
                }

                dbContext.Items.AddRange(newItems);

                RemoveEmptyStrings(dbContext);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }


        #region helpers

        private static void RemoveEmptyStrings(SimpleDataManagementSystemDbContext dbContext)
        {
            // Look for changes
            dbContext.ChangeTracker.DetectChanges();

            // Loop through each entity
            foreach (var entity in dbContext.ChangeTracker.Entries())
            {
                // Use reflection to find editable string properties
                var properties = from p in entity.Entity.GetType().GetProperties()
                                 where p.PropertyType == typeof(string)
                                       && p.CanRead
                                       && p.CanWrite
                                 select p;

                // Loop through each property and replace empty strings with null
                foreach (var property in properties)
                {
                    if (string.IsNullOrWhiteSpace(property.GetValue(entity.Entity, null) as string))
                        property.SetValue(entity.Entity, null, null);
                }
            }
        }

        /// <summary>
        /// Converts an array of randomly generated characters into a string.
        /// </summary>
        /// <param name="maxLength">Final string character length.</param>
        /// <returns>Random string.</returns>
        private static string GenerateRandomDefaultPassword(int maxLength = 16)
        {
            const string ALLOWED_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?#@$^*()";
            Random rnd = new Random();

            char[] chars = new char[maxLength];

            for (int i = 0; i < maxLength; i++)
            {
                chars[i] = ALLOWED_CHARS[rnd.Next(0, ALLOWED_CHARS.Length)];
            }

            var str = new string(chars);

            return str;
        }

        #endregion
    }
}
