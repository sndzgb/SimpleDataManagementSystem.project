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
                IOptions<EmailClientOptions> emailClientOptions
            )
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            
            var created = dbContext.Database.EnsureCreated();

            if (!created) // exists
            {
                return;
            }


            // TODO add navigation properties: roles, claims
            var user = new UserEntity();
            user.Username = "admin"; // TODO generate random username... do the same as with password, and email it both to the user
            user.Id = 0;
            user.CreatedUTC = DateTime.UtcNow;
            user.RoleId = (int)Roles.Admin;

            user.IsPasswordChangeRequired = true;
            //user.Id = 0;
            //user.UserName = "admin";
            //user.EmailConfirmed = false;
            //user.Email = "admin@email.org";
            //user.PhoneNumberConfirmed = false;
            //user.TwoFactorEnabled = false;
            //user.LockoutEnabled = false;
            //user.AccessFailedCount = 5;

            var generatedPassword = GenerateRandomDefaultPassword();

            var hasher = new PasswordHasher<UserEntity>(); //IdentityUser
            user.PasswordHash = hasher.HashPassword(user, generatedPassword);

            // TODO error handling - wrap all db seed functions in try-catch

            SeedRoles(dbContext);

            // SEED users (admin)
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] ON");

                //var admin = new UserEntity();

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] OFF");

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


#if !DEVELOPMENT
            return;
#endif

            SeedRetailers(dbContext);
            SeedCategories(dbContext);
            SeedItems(dbContext);
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


        private static void SeedRoles(SimpleDataManagementSystemDbContext dbContext)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Roles] ON");

                var roles = new List<RoleEntity>();

                var rolesEnum = Enum.GetValues(typeof(Roles)).Cast<Roles>();

                foreach (Roles role in rolesEnum)
                {
                    roles.Add(new RoleEntity()
                    {
                        Id = (int)role,
                        Name = role.ToString(),
                        CreatedUTC = DateTime.UtcNow
                    });
                }

                dbContext.Roles.AddRange(roles);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Roles] OFF");

                transaction.Commit();
            }

        }


        private static void SeedItems(SimpleDataManagementSystemDbContext dbContext)
        {
            //using (dbContext)
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Items] ON");


                var retailers = new List<ItemEntity>();

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

                        //var s = Path.GetFileName(uri.LocalPath);

                        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                        if (response.IsSuccessStatusCode)
                        {
                            using (var fs = new FileStream(Path.Combine(temp, Path.GetFileName(uri.LocalPath)), FileMode.CreateNew)) // TODO check file extension...
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

                    // demo purposes only; saves in "bin" folder
                    //model.URLdoslike = Path.Combine(assDir, "Images\\Items\\" + Path.GetFileName(model.URLdoslike));
                    
                    //model.URLdoslike = Path.Combine("Images\\Items\\" + Path.GetFileName(model.URLdoslike));
                    lItems.Add(model);
                }


                dbContext.Items.AddRange(lItems);

                RemoveEmptyStrings(dbContext);

                dbContext.SaveChanges();

                //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Items] OFF");

                transaction.Commit();
            }
        }


        private static void SeedCategories(SimpleDataManagementSystemDbContext dbContext)
        {
            //using (dbContext)
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] ON");


                var categories = new List<CategoryEntity>();

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

                //dbContext.Categories.AddRange(lCategories);


                dbContext.Categories.AddRange(lCategories);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] OFF");

                transaction.Commit();
            }
        }


        private static void SeedRetailers(SimpleDataManagementSystemDbContext dbContext)
        {
            //using (dbContext)
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Retailers] ON");

                // TODO generate sql script... identity?
                var retailers = new List<RetailerEntity>();

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
                            using (var fs = new FileStream(Path.Combine(temp, model.Name + ext), FileMode.CreateNew))
                            {
                                response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                            }

                            model.LogoImageUrl = Path.Combine("Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
                        }

                    }

                    // demo purposes only; saves in "bin" folder
                    //model.LogoImageUrl = Path.Combine(assDir, "Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
                    //model.LogoImageUrl = Path.Combine("Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
                    lRetailers.Add(model);
                }

                dbContext.Retailers.AddRange(lRetailers);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Retailers] OFF");

                transaction.Commit();
            }
        }
    }
}
