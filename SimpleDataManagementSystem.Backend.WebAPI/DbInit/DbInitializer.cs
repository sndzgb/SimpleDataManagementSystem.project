using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Reflection;
using System.Xml;

namespace SimpleDataManagementSystem.Backend.WebAPI.DbInit
{
    internal class DbInitializer
    {
        internal static void Initialize(SimpleDataManagementSystemDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            
            var created = dbContext.Database.EnsureCreated();

            if (!created) // exists
            {
                return;
            }

            // admin user & roles
            dbContext.Database.ExecuteSqlRaw(@"
SET IDENTITY_INSERT Roles ON
;

INSERT INTO Roles (Id, Name) VALUES (1, 'Admin'), (2, 'Employee'), (3, 'User')
;

SET IDENTITY_INSERT Roles OFF
;

SET IDENTITY_INSERT Users ON
;

INSERT INTO Users(Id, Username, Password, RoleId) VALUES (0, 'admin', 'admin', 1)
;

SET IDENTITY_INSERT Users OFF
;
");


            SeedRetailers(dbContext);
            SeedCategories(dbContext);
            SeedItems(dbContext);

            //dbContext.SaveChanges(); // TODO
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
                        using (var fs = new FileStream(Path.Combine(temp, Path.GetFileName(uri.LocalPath)), FileMode.CreateNew)) // TODO check file extension...
                        {
                            response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                        }
                    }

                    // demo purposes only; saves in "bin" folder
                    //model.URLdoslike = Path.Combine(assDir, "Images\\Items\\" + Path.GetFileName(model.URLdoslike));
                    model.URLdoslike = Path.Combine("Images\\Items\\" + Path.GetFileName(model.URLdoslike));
                    lItems.Add(model);
                }


                dbContext.Items.AddRange(lItems);

                RemoveEmptyStrings(dbContext);

                dbContext.SaveChanges();

                //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Items] OFF");

                transaction.Commit();
            }
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
                        using (var fs = new FileStream(Path.Combine(temp, model.Name + ext), FileMode.CreateNew))
                        {
                            response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                        }
                    }

                    // demo purposes only; saves in "bin" folder
                    //model.LogoImageUrl = Path.Combine(assDir, "Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
                    model.LogoImageUrl = Path.Combine("Images\\Retailers\\" + model.Name + Path.GetExtension(model.LogoImageUrl));
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
