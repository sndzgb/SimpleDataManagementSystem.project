using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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

        // ItemSearchResponseDTO
        public async Task<Tuple<List<Item>?, int>> SearchItemsAsync(ItemSearchRequestDTO request, CancellationToken cancellationToken)
        {
            string whereClause = "";
            string orderByClause = "";

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                whereClause = $@" 
WHERE
( 
    @searchQuery IS NULL
    OR 
    i.Cijena = (select TRY_CONVERT(numeric(18, 2), @searchQuery))
)
OR
(
	@searchQuery IS NULL 
	OR 
	UPPER(i.Nazivproizvoda) LIKE ('%' + UPPER(@searchQuery) + '%')
--set collation
    COLLATE Latin1_general_CI_AI
) 
                ";
            }

            switch (request.SortBy)
            {
                case SearchableItemSortOrder.NazivproizvodaAsc:
                    orderByClause = " i.Nazivproizvoda ASC ";
                    break;
                case SearchableItemSortOrder.NazivproizvodaDesc:
                    orderByClause = " i.Nazivproizvoda DESC ";
                    break;
                case SearchableItemSortOrder.CijenaAsc:
                    orderByClause = " i.Cijena ASC ";
                    break;
                case SearchableItemSortOrder.CijenaDesc:
                    orderByClause = " i.Cijena DESC ";
                    break;
                default:
                    orderByClause = " i.Nazivproizvoda DESC ";
                    break;
            }

            var rawSql =
@$"
SELECT 
	i.Nazivproizvoda
    ,i.Cijena
    ,i.URLdoslike
FROM 
	Items AS i
    {whereClause}
ORDER BY
	{orderByClause}
OFFSET (@page - 1) * @take ROWS
FETCH NEXT @take ROWS ONLY
;
";

            List<ItemEntity> results = new List<ItemEntity>();
            int totalItemsFound = 0;

            using (var connection = _dbContext.Database.GetDbConnection())
            {
                connection.Open();

                var totalsCommand = new SqlCommand($@"
SELECT
COUNT(*) AS 'TotalItemsFound'
FROM 
Items AS i 
WHERE 
i.Nazivproizvoda LIKE ('%'+@nazivproizvoda+'%') 
COLLATE Latin1_general_CI_AI",
                    (SqlConnection)connection
                );
                totalsCommand.Parameters.Add(new SqlParameter("nazivproizvoda", request.SearchQuery));

                var totalItemsFoundReader = totalsCommand.ExecuteReader();

                if (totalItemsFoundReader != null)
                {
                    while (totalItemsFoundReader.Read())
                    {
                        totalItemsFound = totalItemsFoundReader.GetInt32("TotalItemsFound");
                    }
                }


                List<SqlParameter> ps = new List<SqlParameter>();
                ps.Add(new SqlParameter("searchQuery", request.SearchQuery?.ToUpper()));
                ps.Add(new SqlParameter("take", request.Take));
                ps.Add(new SqlParameter("page", request.Page));

                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = rawSql;
                command.Parameters.AddRange(ps.ToArray());

                var reader = command.ExecuteReader();

                if (reader != null)
                { 
                    while (reader.Read())
                    {
                        var item = new ItemEntity();

                        item.Nazivproizvoda = reader.GetString("Nazivproizvoda");
                        item.Cijena = reader.GetDecimal("Cijena");
                        item.URLdoslike = reader.IsDBNull("URLdoslike") ? null : reader.GetString("URLdoslike");

                        results.Add(item);
                    }

                    reader.Close();
                }

                command.Dispose();
                connection.Close();
            }

            // TODO join "Retailer"
            var model = new List<Item>();

            if (results != null)
            {
                model.AddRange(
                    results
                        .Select(x => new Item()
                        {
                            Cijena = x.Cijena,
                            Nazivproizvoda = x.Nazivproizvoda,
                            URLdoslike = x.URLdoslike
                        })
                        .ToList()
                );

            }

            return new Tuple<List<Item>?, int>(model, totalItemsFound);
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
