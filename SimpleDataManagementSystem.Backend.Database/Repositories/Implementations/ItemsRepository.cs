using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public ItemsRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<string> AddNewItemAsync(NewItemDTO newItemDTO)
        {
            if (newItemDTO == null)
            {
                throw new ArgumentNullException(nameof(newItemDTO));
            }

            var newItem = await _dbContext.Items.AddAsync(new ItemEntity()
            {
                RetailerID = newItemDTO.RetailerId,
                Opis = newItemDTO.Opis,
                Cijena = newItemDTO.Cijena,
                Datumakcije = newItemDTO.Datumakcije,
                Kategorija = newItemDTO.Kategorija,
                Nazivproizvoda = newItemDTO.Nazivproizvoda,
                URLdoslike = newItemDTO.URLdoslike
            });

            await _dbContext.SaveChangesAsync();

            return newItem.Entity.Nazivretailera;
        }

        public async Task<List<Item>?> GetItemsByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return null;
            }

            var entities = await _dbContext.Items
                .Include(x => x.Retailer)
                .Include(x => x.Category)
                .Where(x => x.Nazivproizvoda == title)
                .ToListAsync();

            if (entities == null)
            {
                return null;
            }

            var items = Map(entities);

            return items;
        }

        public async Task DeleteItemAsync(string itemId)
        {
            if (itemId == null)
            {
                return;
            }

            var item = await _dbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return;
            }

            _dbContext.Items.Remove(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ItemsDTO?> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            var total = await _dbContext.Items.CountAsync();

            var items = new ItemsDTO();

            items.PageInfo.Total = total;
            items.PageInfo.Take = (int)take!;
            items.PageInfo.Page = (int)page!;

            if (total > 0)
            {
                items.Items.AddRange
                (
                    await _dbContext.Items
                        .OrderBy(x => x.Nazivproizvoda)
                        .Skip((page!.Value - 1) * take!.Value)
                        .Take(take.Value)
                        .Select(s => new ItemDTO()
                        {
                            Nazivproizvoda = s.Nazivproizvoda,
                            URLdoslike = s.URLdoslike,
                            Nazivretailera = s.Nazivretailera,
                            Kategorija = s.Kategorija,
                            Datumakcije = s.Datumakcije,
                            Cijena = s.Cijena,
                            Opis = s.Opis
                        })
                        .ToListAsync()
                );
            }

            return items;
        }

        public async Task<ItemDTO?> GetItemByIdAsync(string itemId)
        {
            var item = await _dbContext.Items.Include(x => x.Retailer).SingleOrDefaultAsync(x => x.Nazivproizvoda == itemId);

            if (item == null)
            {
                return null;
            }

            return new ItemDTO()
            {
                RetailerId = item.RetailerID,
                Opis = item.Opis,
                Cijena = item.Cijena,
                Datumakcije = item.Datumakcije,
                Kategorija = item.Kategorija,
                Nazivproizvoda = item.Nazivproizvoda,
                URLdoslike = item.URLdoslike,
                Nazivretailera = item.Nazivretailera
            };
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO)
        {
            if (updateItemDTO == null)
            {
                return;
            }

            var item = await _dbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return;
            }

            var retailer = await _dbContext.Retailers.FindAsync(updateItemDTO.RetailerId);

            if (retailer == null) // if retailer does not exist, item has to have retailer; throw exception?
            {
                return;
            }

            if (updateItemDTO.URLdoslike != null)
            {
                item.URLdoslike = updateItemDTO.URLdoslike;
            }

            item.Opis = updateItemDTO.Opis;
            item.RetailerID = updateItemDTO.RetailerId;
            item.Cijena = updateItemDTO.Cijena;
            item.Datumakcije = updateItemDTO.Datumakcije;
            item.Kategorija = updateItemDTO.Kategorija;
            item.RetailerID = updateItemDTO.RetailerId;
            item.Nazivretailera = retailer.Name;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateItemPartialAsync(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }

            var item = await _dbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return;
            }

            item.URLdoslike = null;

            await _dbContext.SaveChangesAsync();
        }


        #region Mapping

        private List<Item>? Map(List<ItemEntity>? entities)
        {
            if (entities == null)
            {
                return null;
            }

            var items = new List<Item>();

            foreach (var entity in entities)
            {
                var item = new Item()
                {
                    Cijena = entity.Cijena,
                    Datumakcije = entity.Datumakcije,
                    Nazivproizvoda = entity.Nazivproizvoda,
                    Nazivretailera = entity.Nazivretailera,
                    Opis = entity.Opis,
                    URLdoslike = entity.URLdoslike
                };

                if (entity.Category != null)
                {
                    item.Category = new Category()
                    {
                        ID = entity.Category.ID,
                        Name = entity.Category.Name,
                        Priority = entity.Category.Priority
                    };
                }

                if (entity.Retailer != null)
                {
                    item.Retailer = new Retailer()
                    {
                        ID = entity.Retailer.ID,
                        LogoImageUrl = entity.Retailer.LogoImageUrl,
                        Name = entity.Retailer.LogoImageUrl,
                        Priority = entity.Retailer.Priority
                    };
                }

                items.Add(item);
            }

            return items;
        }

        #endregion
    }
}
