using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
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
            var newItem = await _dbContext.Items.AddAsync(new ItemEntity()
            {
                Nazivretailera = newItemDTO.Nazivretailera,
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

        public async Task<List<ItemDTO>> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            return await _dbContext.Items.OrderBy(x => x.Nazivretailera)
                .Skip((page!.Value - 1) * take!.Value)
                .Take(take.Value)
                .Select(s => new ItemDTO()
                {
                    Nazivretailera = s.Nazivretailera,
                    URLdoslike = s.URLdoslike,
                    Nazivproizvoda = s.Nazivproizvoda,
                    Kategorija = s.Kategorija,
                    Datumakcije = s.Datumakcije,
                    Cijena = s.Cijena,
                    Opis = s.Opis
                })
                .ToListAsync();
        }

        public async Task<ItemDTO?> GetItemByIdAsync(string itemId)
        {
            var item = await _dbContext.Items.FindAsync(itemId);

            return new ItemDTO()
            {
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
            var item = await _dbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return;
            }

            item.Opis = updateItemDTO.Opis;
            item.Cijena = updateItemDTO.Cijena;
            item.Datumakcije = updateItemDTO.Datumakcije;
            item.URLdoslike = updateItemDTO.URLdoslike;
            item.Nazivproizvoda = updateItemDTO.Nazivproizvoda;
            item.Kategorija = updateItemDTO.Kategorija;
            item.Nazivretailera = updateItemDTO.Nazivretailera;

            if (!string.IsNullOrEmpty(updateItemDTO.URLdoslike))
            {
                item.URLdoslike = updateItemDTO.URLdoslike;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
