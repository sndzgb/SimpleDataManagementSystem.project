using Azure.Core;
using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class ItemsCoreRepository : IItemsCoreRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;
        private readonly IDbContextFactory<SimpleDataManagementSystemDbContext> _dbContextFactory;

        public ItemsCoreRepository(
                SimpleDataManagementSystemDbContext dbContext, 
                IDbContextFactory<SimpleDataManagementSystemDbContext> dbContextFactory
            )
        {
            _dbContext = dbContext;
            _dbContextFactory = dbContextFactory;
        }


        public async Task<Item?> GetItemAsync(GetItemRequestDTO dto, CancellationToken cancellationToken)
        {
            var queryable = _dbContext.Items.Where(x => x.Nazivproizvoda == dto.ItemId).AsQueryable();

            if (dto.IncludeRetailer)
            {
                queryable = queryable.Include(x => x.Retailer);
            }

            if (dto.IncludeCategory)
            {
                queryable = queryable.Include(x => x.Category);
            }

            if (dto.Monitoring.IncludeMonitoring)
            {
                queryable = queryable
                    .Include(x => x.Monitored);
                    //.Skip(dto.Monitoring.Take * (dto.Page - 1))
                    //.Take(dto.Monitoring.Page);
            }

            var result = await queryable
                .Select(
                    x => 
                        new Item(
                        //x.Nazivproizvoda, 
                        //x.Cijena, 
                        //new Retailer(
                        //    x.Retailer.ID, 
                        //    x.Retailer.Name,
                        //    x.Retailer.Priority
                        //)
                        )
                        {
                            Category = new Category()
                            {
                                Name = x.Category.Name,
                                Priority = x.Category.Priority,
                                ID = x.Category.ID
                            },
                            Cijena = x.Cijena,
                            Retailer = new Retailer()
                            {
                                Priority = x.Retailer.Priority,
                                Name = x.Retailer.Name,
                                LogoImageUrl = x.Retailer.LogoImageUrl,
                                ID = x.Retailer.ID
                            },
                            Datumakcije = x.Datumakcije,
                            IsEnabled = x.IsEnabled,
                            //LastUpdatedByUserId = x.LastUpdatedByUserId,
                            //LastUpdatedUtc = x.LastUpdatedUtc,
                            Nazivproizvoda = x.Nazivproizvoda,
                            Opis = x.Opis,
                            URLdoslike = x.URLdoslike,
                            Nazivretailera = null,
                            Monitored = x.Monitored.Select(x => new MonitoredItem()
                            {
                                Item = new Item()
                                {
                                    Nazivproizvoda = x.Item.Nazivproizvoda,
                                },
                                User = new User()
                                {
                                    ID = x.User.Id
                                },
                                StartedMonitoringAtUtc = x.StartedMonitoringAtUtc
                            }).ToList()
                        }
            ).FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        //public async Task<IEnumerable<Item>?> GetItemsAsync(GetItemsRequestDTO getItemsRequestDTO, CancellationToken cancellationToken)
        //{
        //    var queryable = _dbContext.Items;

        //    if (getItemsRequestDTO.RequestParameters.IncludeRetailer)
        //    {
        //        queryable.Include(x => x.Retailer);
        //    }

        //    if (getItemsRequestDTO.RequestParameters.IncludeCategory)
        //    {
        //        queryable.Include(x => x.Category);
        //    }

        //    switch (getItemsRequestDTO.RequestParameters.SortBy)
        //    {
        //        case GetItemsRequestDTO.RequestDTO.Sort.NazivproizvodaAsc:
        //            queryable.OrderBy(x => x.Nazivproizvoda);
        //            break;
        //        case GetItemsRequestDTO.RequestDTO.Sort.NazivproizvodaDesc:
        //            queryable.OrderByDescending(x => x.Nazivproizvoda);
        //            break;
        //        case GetItemsRequestDTO.RequestDTO.Sort.CijenaAsc:
        //            queryable.OrderBy(x => x.Cijena);
        //            break;
        //        case GetItemsRequestDTO.RequestDTO.Sort.CijenaDesc:
        //            queryable.OrderByDescending(x => x.Cijena);
        //            break;
        //        default:
        //            throw new NotImplementedException();
        //    }

        //    var result = await queryable
        //        .Skip(getItemsRequestDTO.RequestParameters.Take * (getItemsRequestDTO.RequestParameters.Page - 1))
        //        .Take(getItemsRequestDTO.RequestParameters.Take)
        //        .Select(
        //            x => 
        //                new Item
        //                (
        //                    x.Nazivproizvoda, 
        //                    x.Cijena, 
        //                    new Retailer(x.Retailer.ID, x.Retailer.Name, x.Retailer.Priority)
        //                    {
        //                        LogoImageUrl = x.Retailer.LogoImageUrl,
        //                    }
        //                )
        //                {
        //                    Datumakcije = x.Datumakcije,
        //                    URLdoslike = x.URLdoslike,
        //                    IsEnabled = x.IsEnabled
        //                }
        //        )
        //        .ToListAsync(cancellationToken);

        //    return result;
        //}

        public async Task<ItemsDTO> GetItemsAsync(GetItemsRequestDTO getItemsRequestDTO, CancellationToken cancellationToken)
        {
            //var items = await _dbContext
            //    .Items
            //    .Include(x => x.Retailer)
            //    .Include(x => x.Category)
            //    .Include(x => x.Monitored)
            //    .ToListAsync(cancellationToken);

            using var itemsDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
			var itemsQueryable = itemsDbContext.Items.AsQueryable();

			// get items -- COMMENTED
			//var itemsQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken)).Items.AsQueryable();



			//if (getItemsRequestDTO.IncludeRetailer)
			//{
			//    itemsQueryable.Include(x => x.Retailer);
			//}

			//if (getItemsRequestDTO.IncludeCategory)
			//{
			//    itemsQueryable.Include(x => x.Category);
			//}

			//if (getItemsRequestDTO.Monitoring.IncludeMonitoring)
			//{
			//    itemsQueryable.Include(
			//        x =>
			//            x.Monitored
			//                .Where(x => x.UserId == getItemsRequestDTO.RequestedByUserId)
			//    );
			//}


			var includeQueryable = itemsQueryable
                        .Include(x => x.Retailer)
                        .Include(x => x.Category)
                        .Include(x => x.Monitored);

            itemsQueryable = includeQueryable;

            IOrderedQueryable<ItemEntity> sortQueryable = null;

            switch (getItemsRequestDTO.PageInfo.SortBy)
            {
                case Logic.DTOs.SortableItem.NazivproizvodaAsc:
                    sortQueryable = itemsQueryable.OrderBy(x => x.Nazivproizvoda);
                    break;
                case Logic.DTOs.SortableItem.NazivproizvodaDesc:
                    sortQueryable = itemsQueryable.OrderByDescending(x => x.Nazivproizvoda);
                    break;
                case Logic.DTOs.SortableItem.CijenaAsc:
                    sortQueryable = itemsQueryable.OrderBy(x => x.Cijena);
                    break;
                case Logic.DTOs.SortableItem.CijenaDesc:
                    sortQueryable = itemsQueryable.OrderByDescending(x => x.Cijena);
                    break;
                default:
                    sortQueryable = itemsQueryable.OrderBy(x => x.Nazivproizvoda);
                    break;
            }

            itemsQueryable = sortQueryable;

            var onlyEnabledQueryable = itemsQueryable.Where(x => x.IsEnabled || (x.IsEnabled == getItemsRequestDTO.EnabledOnly));

            itemsQueryable = onlyEnabledQueryable;

            var paginationQueryable = itemsQueryable
                                                .Skip(getItemsRequestDTO.PageInfo.Take * (getItemsRequestDTO.PageInfo.Page - 1))
                                                .Take(getItemsRequestDTO.PageInfo.Take);

            itemsQueryable = paginationQueryable;

            itemsQueryable
                        .Select(
                            x =>
                                new Item()
                                {
                                    Nazivproizvoda = x.Nazivproizvoda,
                                    Cijena = x.Cijena,
                                    Retailer = new Retailer()
                                    {
                                        ID = x.Retailer.ID,
                                        Name = x.Retailer.Name,
                                        Priority = x.Retailer.Priority,
                                        LogoImageUrl = x.Retailer.LogoImageUrl
                                    },
                                    Datumakcije = x.Datumakcije,
                                    URLdoslike = x.URLdoslike,
                                    IsEnabled = x.IsEnabled,
                                    Category = new Category()
                                    {
                                        ID = x.Category.ID,
                                        Name = x.Category.Name,
                                        Priority = x.Category.Priority
                                    },
                                    //LastUpdatedByUserId = x.LastUpdatedByUserId,
                                    //LastUpdatedUtc = x.LastUpdatedUtc,
                                    Opis = x.Opis,
                                    Nazivretailera = null,
                                    Monitored = (List<MonitoredItem>)x.Monitored.Select(m => new MonitoredItem()
                                    {
                                        User = new User()
                                        {
                                            ID = m.UserId
                                        },
                                        StartedMonitoringAtUtc = m.StartedMonitoringAtUtc,
                                        Item = new Item()
                                        {
                                            Nazivproizvoda = m.ItemNazivproizvoda
                                        }
                                    })
                                }
                        );


            // count -- COMMENTED
            //var countQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            //    .Items
            //    .Where(x => x.IsEnabled || (x.IsEnabled == getItemsRequestDTO.EnabledOnly));
            using var countDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var countQueryable = countDbContext
                                            .Items
								            .Where(x => x.IsEnabled || (x.IsEnabled == getItemsRequestDTO.EnabledOnly));

			//.Skip(getItemsRequestDTO.PageInfo.Take * (getItemsRequestDTO.PageInfo.Page - 1))
			//.Take(getItemsRequestDTO.PageInfo.Take);

			var countTask = countQueryable.CountAsync(cancellationToken);
            var itemsTask = itemsQueryable.ToListAsync(cancellationToken);

            await Task.WhenAll(countTask, itemsTask);

            var countTaskResult = await countTask;
            var itemsTaskResult = await itemsTask;

            var response = new ItemsDTO();
            response.Items = new List<Item>();
            response.PageInfo = new ItemsDTO.PageDTO();

            response.Items.AddRange(itemsTaskResult.Select(x => new Item()
            {
                Category = new Category()
                {
                    ID = x.Category.ID,
                    Name = x.Category.Name,
                    Priority = x.Category.Priority
                },
                Cijena = x.Cijena,
                Datumakcije = x.Datumakcije,
                IsEnabled = x.IsEnabled,
                //LastUpdatedByUserId = x.LastUpdatedByUserId,
                //LastUpdatedUtc = x.LastUpdatedUtc,
                Nazivproizvoda = x.Nazivproizvoda,
                Nazivretailera = null,
                Opis = x.Opis,
                Retailer = new Retailer()
                {
                    ID = x.Retailer.ID,
                    Name = x.Retailer.Name,
                    LogoImageUrl = x.Retailer.LogoImageUrl,
                    Priority = x.Retailer.Priority
                },
                URLdoslike = x.URLdoslike,
                Monitored = x.Monitored.Select(x => new MonitoredItem()
                {
                    Item = new Item()
                    {
                        Nazivproizvoda = x.ItemNazivproizvoda
                    },
                    User = new User()
                    {
                        ID = x.UserId
                    },
                    StartedMonitoringAtUtc = x.StartedMonitoringAtUtc
                }).ToList()
            }));

            response.PageInfo.Total = countTaskResult;





            //var queryable = _dbContext.Items;

            //if (getItemsRequestDTO.IncludeRetailer)
            //{
            //    queryable.Include(x => x.Retailer);
            //}

            //if (getItemsRequestDTO.IncludeCategory)
            //{
            //    queryable.Include(x => x.Category);
            //}

            //if (getItemsRequestDTO.Monitoring.IncludeMonitoring)
            //{
            //    queryable.Include(
            //        x =>
            //            x.Monitored
            //                .Where(x => x.UserId == getItemsRequestDTO.RequestedByUserId)
            //    );
            //}

            //switch (getItemsRequestDTO.PageInfo.SortBy)
            //{
            //    case Logic.DTOs.SortableItem.NazivproizvodaAsc:
            //        break;
            //    case Logic.DTOs.SortableItem.NazivproizvodaDesc:
            //        break;
            //    case Logic.DTOs.SortableItem.CijenaAsc:
            //        break;
            //    case Logic.DTOs.SortableItem.CijenaDesc:
            //        break;
            //    default:
            //        break;
            //}

            //queryable
            //    .Select(
            //        x =>
            //            new Item()
            //            {
            //                Nazivproizvoda = x.Nazivproizvoda,
            //                Cijena = x.Cijena,
            //                Retailer = new Retailer()
            //                {
            //                    ID = x.Retailer.ID,
            //                    Name = x.Retailer.Name,
            //                    Priority = x.Retailer.Priority,
            //                    LogoImageUrl = x.Retailer.LogoImageUrl
            //                },
            //                Datumakcije = x.Datumakcije,
            //                URLdoslike = x.URLdoslike,
            //                IsEnabled = x.IsEnabled,
            //                Category = new Category()
            //                {
            //                    ID = x.Category.ID,
            //                    Name = x.Category.Name,
            //                    Priority = x.Category.Priority
            //                },
            //                LastUpdatedByUserId = x.LastUpdatedByUserId,
            //                LastUpdatedUtc = x.LastUpdatedUtc,
            //                Opis = x.Opis,
            //                Nazivretailera = null,
            //                Monitored = x.Monitored.Select(m => new MonitoredItem()
            //                {
            //                    StartedMonitoringAtUtc = m.StartedMonitoringAtUtc,
            //                }).ToList()
            //            }
            //    );

            //var itemsQueryable = queryable
            //                    .Skip(getItemsRequestDTO.PageInfo.Take * (getItemsRequestDTO.PageInfo.Page - 1))
            //                    .Take(getItemsRequestDTO.PageInfo.Take);
            //                    //.ToListAsync(cancellationToken);

            ////var collection = await queryable.ToListAsync(cancellationToken);

            //var countTask = queryable.CountAsync(cancellationToken);
            //var itemsTask = itemsQueryable.ToListAsync(cancellationToken);

            //await Task.WhenAll(countTask, itemsTask);

            //var countTaskResult = await countTask;
            //var itemsTaskResult = await itemsTask;

            //var response = new ItemsDTO();
            //response.Items = new List<Item>();
            //response.PageInfo = new ItemsDTO.PageDTO();

            //response.Items.AddRange(itemsTaskResult.Select(x => new Item()
            //{
            //    Category = new Category()
            //    {
            //        ID = x.Category.ID,
            //        Name = x.Category.Name,
            //        Priority = x.Category.Priority
            //    },
            //    Cijena = x.Cijena,
            //    Datumakcije = x.Datumakcije,
            //    IsEnabled = x.IsEnabled,
            //    LastUpdatedByUserId = x.LastUpdatedByUserId,
            //    LastUpdatedUtc = x.LastUpdatedUtc,
            //    Nazivproizvoda = x.Nazivproizvoda,
            //    Nazivretailera = null,
            //    Opis = x.Opis,
            //    Retailer = new Retailer()
            //    {
            //        ID = x.Retailer.ID,
            //        Name = x.Retailer.Name,
            //        LogoImageUrl = x.Retailer.LogoImageUrl,
            //        Priority = x.Retailer.Priority
            //    },
            //    URLdoslike = x.URLdoslike,
            //    Monitored = x.Monitored.Select(x => new MonitoredItem()
            //    {
            //        StartedMonitoringAtUtc = x.StartedMonitoringAtUtc
            //    }).ToList()
            //}));

            //response.PageInfo.Total = countTaskResult;








            //var response = new GetItemsResponseDTO();
            //response.Items = new List<GetItemsResponseDTO.ItemDTO>();

            //response.Items.AddRange(itemsTaskResult.Select(x => new GetItemsResponseDTO.ItemDTO()
            //{
            //    Category = new GetItemsResponseDTO.ItemDTO.CategoryDTO()
            //    {
            //        Id = x.Category.ID,
            //        Name = x.Category.Name
            //    },
            //    Cijena = x.Cijena,
            //    Datumakcije = x.Datumakcije,
            //    IsEnabled = x.IsEnabled,
            //    IsMonitoredByCurrentUser = x.Monitored?.FirstOrDefault() != null,
            //    Nazivproizvoda = x.Nazivproizvoda,
            //    Opis = x.Opis,
            //    Retailer = new GetItemsResponseDTO.ItemDTO.RetailerDTO()
            //    {
            //        Id = x.Retailer.ID,
            //        Name = x.Retailer.Name,
            //        Priority = x.Retailer.Priority
            //    },
            //    URLdoslike = x.URLdoslike
            //}));

            //response.Page.Total = countTaskResult;

            return response;
        }

        public async Task UpdateItemAsync(UpdateItemRequestDTO updateItemRequestDTO, CancellationToken cancellationToken)
        {
            var existingItem = await _dbContext
                .Items
                .Include(x => x.Monitored)
                .Where(x => x.Nazivproizvoda == updateItemRequestDTO.RequestMetadata.ItemId).FirstOrDefaultAsync(cancellationToken);

            if (existingItem == null)
            {
                return;
            }

            existingItem.Opis = updateItemRequestDTO.Opis;
            existingItem.RetailerID = updateItemRequestDTO.RetailerId;
            //existingItem.Retailer = new RetailerEntity()
            //{
            //    ID = updateItemRequestDTO.RetailerId
            //};
            existingItem.Cijena = updateItemRequestDTO.Cijena;
            existingItem.Nazivretailera = null;
            existingItem.IsEnabled = updateItemRequestDTO.IsEnabled;
            existingItem.Datumakcije = updateItemRequestDTO.Datumakcije;
            existingItem.Kategorija = updateItemRequestDTO.CategoryId;
            //existingItem.Category = new CategoryEntity()
            //{
            //    ID = updateItemRequestDTO.CategoryId
            //};
            //existingItem.LastUpdatedByUserId = updateItemRequestDTO.RequestedByUserId;
            //existingItem.LastUpdatedUtc = DateTime.UtcNow;
            
            if (!updateItemRequestDTO.IsMonitoredByUser)
            {
                var item = existingItem.Monitored.Where(x => x.UserId == updateItemRequestDTO.RequestedByUserId).FirstOrDefault();
                
                if (item != null)
                {
                    _dbContext.MonitoredItems.Remove(item);
                }
            }
            else
            {
                // add like removing up?
                existingItem.Monitored.Add(new MonitoredItemEntity()
                {
                    //User = new UserEntity()
                    //{
                    //    Id = updateItemRequestDTO.RequestedByUserId
                    //},
                    //Item = new ItemEntity()
                    //{
                    //    Nazivproizvoda = existingItem.Nazivproizvoda
                    //},
                    StartedMonitoringAtUtc = DateTime.UtcNow,
                    UserId = updateItemRequestDTO.RequestedByUserId,
                    ItemNazivproizvoda = existingItem.Nazivproizvoda
                });
            }

            if (updateItemRequestDTO.DeleteCurrentURLdoslike)
            {
                existingItem.URLdoslike = null;
            }
            
            if (!string.IsNullOrEmpty(updateItemRequestDTO.URLdoslike))
            {
                existingItem.URLdoslike = updateItemRequestDTO.URLdoslike;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteItemAsync(DeleteItemRequestDTO deleteItemRequestDTO, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Items.FindAsync(deleteItemRequestDTO.Metadata.ItemId, cancellationToken);

            if (item == null)
            {
                return;
            }

            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Item> CreateItemAsync(CreateItemRequestDTO createItemRequestDTO, CancellationToken cancellationToken)
        {
            var newItem = await _dbContext.Items.AddAsync(new Entities.ItemEntity()
            {
                Cijena = createItemRequestDTO.Cijena,
                Datumakcije = createItemRequestDTO.Datumakcije,
                IsEnabled = createItemRequestDTO.IsEnabled,
                //LastUpdatedByUserId = null,
                //LastUpdatedUtc = null,
                Monitored = null,
                Nazivproizvoda = createItemRequestDTO.Nazivproizvoda,
                Nazivretailera = null,
                Opis = createItemRequestDTO.Opis,
                URLdoslike = createItemRequestDTO.URLdoslike,
                Kategorija = createItemRequestDTO.Kategorija,
                RetailerID = createItemRequestDTO.RetailerId
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var newEntity = await _dbContext.Items.Include(x => x.Category).Include(x => x.Retailer)
                .Where(x => x.Nazivproizvoda == newItem.Entity.Nazivproizvoda).FirstOrDefaultAsync(cancellationToken);

            var item = new Item()
            {
                Category = new Category()
                {
                    ID = newEntity.Category.ID,
                    Items = null,
                    Name = newEntity.Category.Name,
                    Priority = newEntity.Category.Priority
                },
                Retailer = new Retailer()
                {
                    Priority = newEntity.Retailer.Priority,
                    Name = newEntity.Retailer.Name,
                    ID = newEntity.Retailer.ID,
                    LogoImageUrl = newEntity.Retailer.LogoImageUrl
                },
                Cijena = newItem.Entity.Cijena,
                Datumakcije = newItem.Entity.Datumakcije,
                IsEnabled = newItem.Entity.IsEnabled,
                //LastUpdatedByUserId = newItem.Entity.LastUpdatedByUserId,
                //LastUpdatedUtc = newItem.Entity.LastUpdatedUtc,
                Monitored = null,
                Nazivproizvoda = newItem.Entity.Nazivproizvoda,
                Nazivretailera = null,
                Opis = newItem.Entity.Opis,
                URLdoslike = newItem.Entity.URLdoslike
            };

            return item;
        }

        public async Task<SearchItemsDTO?> SearchItemsAsync(
                SearchItemsRequestDTO searchItemsRequestDTO, 
                CancellationToken cancellationToken
            )
        {
            if (string.IsNullOrWhiteSpace(searchItemsRequestDTO.PageInfo.Query))
            {
                return null;
            }



            // TODO MARS
            //var dataset = new DataSet();
            //var adapter = new SqlDataAdapter();

            //var command = new SqlCommand(sql, connection);
            //command.Parameters.AddWithValue("@id", 1);
            //adapter.SelectCommand = command;

            //adapter.Fill(dataset);

            //var people = dataset.Tables[0].ToList<Person>();
            //var food = dataset.Tables[1].ToList<Food>();




            string orderByClause = string.Empty;

            switch (searchItemsRequestDTO.PageInfo.SortBy)
            {
                case SortableItem.NazivproizvodaAsc:
                    orderByClause = " i.Nazivproizvoda ASC ";
                    break;
                case SortableItem.NazivproizvodaDesc:
                    orderByClause = " i.Nazivproizvoda DESC ";
                    break;
                case SortableItem.CijenaAsc:
                    orderByClause = " i.Cijena ASC ";
                    break;
                case SortableItem.CijenaDesc:
                    orderByClause = " i.Cijena DESC ";
                    break;
                default:
                    orderByClause = " i.Nazivproizvoda DESC ";
                    break;
            }


            var sqlCountItemsQuery = $@"
SELECT 
	COUNT(*) AS 'TotalItemsFound'
FROM 
	Items AS i
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
    --set collation as accent insensitive (AI)
    COLLATE Latin1_general_CI_AI
) 
AND
(
    i.IsEnabled = 1
    OR
    i.IsEnabled = @isEnabled
)
            ";


            var sqlItemsQuery = @$"
WITH cte_UserMonitoredItems(ItemId)
AS
(
	SELECT
		mi.ItemNazivproizvoda
	FROM
		MonitoredItems AS mi
	LEFT JOIN
		Users AS u
	ON
		mi.UserId = u.id
	WHERE 
		u.Id = @userId
)
SELECT 
	i.Nazivproizvoda
	,i.Opis
	,i.Datumakcije
	,i.URLdoslike
	,i.Cijena
	,i.IsEnabled
	,c.ID AS 'Kategorija'
	,r.ID AS 'RetailerID'
	--,COUNT(*) AS 'TotalUsersMonitoring'
	,CASE 
		WHEN cte.ItemId IS NOT NULL THEN 1
		ELSE 0
	END AS 'IsMonitoredByUser'
FROM 
	Items AS i
LEFT JOIN
	MonitoredItems AS mi
ON
	i.Nazivproizvoda = mi.ItemNazivproizvoda
LEFT JOIN
	cte_UserMonitoredItems AS cte
ON
	i.Nazivproizvoda = cte.ItemId
LEFT JOIN
	Categories AS c
ON
	i.Kategorija = c.ID
LEFT JOIN
	Retailers AS r
ON
	i.RetailerID = r.ID
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
    --set collation as accent insensitive (AI)
    COLLATE Latin1_general_CI_AI
) 
AND
(
    i.IsEnabled = 1
    OR
    i.IsEnabled = @isEnabled
)
GROUP BY
	i.Nazivproizvoda
	,cte.ItemId
	,i.Opis
	,i.Datumakcije
	,i.URLdoslike
	,i.Cijena
	,i.IsEnabled
	,c.ID
	,r.ID
ORDER BY 
	{orderByClause}
OFFSET (@page - 1) * @take ROWS
FETCH NEXT @take ROWS ONLY
;


--SELECT 
--	i.Nazivproizvoda
--    ,i.Cijena
--    ,i.URLdoslike
--    ,i.IsEnabled
--FROM 
--	Items AS i
--      whereClause
--ORDER BY
--      orderByClause
--OFFSET (@page - 1) * @take ROWS
--FETCH NEXT @take ROWS ONLY
            ";

            var countItemsParameters = new Dictionary<string, object>();
            countItemsParameters.Add("searchQuery", searchItemsRequestDTO.PageInfo.Query);
            countItemsParameters.Add("isEnabled", searchItemsRequestDTO.PageInfo.IsEnabled);

            var searchItemsParameters = new Dictionary<string, object>();
            searchItemsParameters.Add("userId", searchItemsRequestDTO.RequestedByUserId);
            searchItemsParameters.Add("page", searchItemsRequestDTO.PageInfo.Page);
            searchItemsParameters.Add("take", searchItemsRequestDTO.PageInfo.Take);
            searchItemsParameters.Add("searchQuery", searchItemsRequestDTO.PageInfo.Query);
            searchItemsParameters.Add("isEnabled", searchItemsRequestDTO.PageInfo.IsEnabled);

            var countItemsTask = GetSearchItemsCountAsync(countItemsParameters, sqlCountItemsQuery, cancellationToken);
            var searchItemsTask = GetSearchItemsAsync(searchItemsParameters, sqlItemsQuery, cancellationToken);

            await Task.WhenAll(countItemsTask, searchItemsTask);

            var countTaskResult = await countItemsTask;
            var searchItemsTaskResult = await searchItemsTask;

            //var response = new SearchItemsResponseDTO();
            var response = new SearchItemsDTO();
            
            response.PageInfo = new SearchItemsDTO.PageDTO();
            response.PageInfo.Total = countTaskResult;

            response.Items = new List<Item>();
            response.Items.AddRange(searchItemsTaskResult.Select(x => new Item()
            {
                Retailer = new Retailer()
                {
                    ID = x.Retailer.ID
                },
                Category = new Category()
                {
                    ID = x.Category.ID
                },
                Cijena = x.Cijena,
                Datumakcije = x.Datumakcije,
                Opis = x.Opis,
                Nazivretailera = null,
                IsEnabled = x.IsEnabled,
                //LastUpdatedByUserId = x.LastUpdatedByUserId,
                //LastUpdatedUtc = x.LastUpdatedUtc,
                Nazivproizvoda = x.Nazivproizvoda,
                URLdoslike = x.URLdoslike,
                Monitored = x.Monitored
            }));

            return response;
        }

        public async Task ToggleItemEnabledDisabledStatusAsync(
                ToggleItemEnabledDisabledStatusRequestDTO toggleItemEnabledDisabledStatusRequestDTO, 
                CancellationToken cancellationToken
            )
        {
            var item = await _dbContext.Items.FindAsync(toggleItemEnabledDisabledStatusRequestDTO.Metadata.ItemId, cancellationToken);

            if (item == null)
            {
                return;
            }

            item.IsEnabled = !item.IsEnabled;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ToggleMonitoredItemAsync(ToggleMonitoredItemRequestDTO toggleMonitoredItemRequestDTO, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Items
                .Include(x => 
                    x.Monitored.Where(y => 
                        y.UserId == toggleMonitoredItemRequestDTO.RequestedByUserId
                        &&
                        y.ItemNazivproizvoda == toggleMonitoredItemRequestDTO.RequestMetadata.ItemId
                    )
                )
                .Where(x => x.Nazivproizvoda == toggleMonitoredItemRequestDTO.RequestMetadata.ItemId)
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                return;
            }

            if (item.Monitored == null || item.Monitored.Count == 0)
            {
                // is not monitored - add
                await _dbContext.MonitoredItems.AddAsync(new MonitoredItemEntity()
                {
                    ItemNazivproizvoda = toggleMonitoredItemRequestDTO.RequestMetadata.ItemId,
                    UserId = toggleMonitoredItemRequestDTO.RequestedByUserId,
                    StartedMonitoringAtUtc = DateTime.UtcNow
                }, cancellationToken);
            }
            else 
            {
                // is monitored - remove
                var m = item.Monitored.FirstOrDefault();
                _dbContext.MonitoredItems.Remove(m);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        #region Helpers

        private async Task<List<Item>> GetSearchItemsAsync(
                Dictionary<string, object> parameters, 
                string sql,
                CancellationToken cancellationToken
            )
        {
            var items = new List<Item>();

            using (var connection = _dbContextFactory.CreateDbContext().Database.GetDbConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;

                //var searchItemsCommand = new SqlCommand();
                //searchItemsCommand.CommandText = sql;
                //searchItemsCommand.CommandType = CommandType.Text;

                var queryParameters = new List<SqlParameter>();
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        queryParameters.Add(new SqlParameter(p.Key, p.Value));
                    }
                }

                command.Parameters.AddRange(queryParameters.ToArray());

                var reader = await command.ExecuteReaderAsync(cancellationToken);

                if (reader != null)
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var item = new Item();

                        item.Nazivproizvoda = reader.GetString("Nazivproizvoda");
                        item.Opis = reader.IsDBNull("Opis") ? null : reader.GetString("Opis");
                        item.Datumakcije = reader.IsDBNull("Datumakcije") ? null : reader.GetString("Datumakcije");
                        item.URLdoslike = reader.IsDBNull("URLdoslike") ? null : reader.GetString("URLdoslike");
                        item.Cijena = reader.GetDecimal("Cijena");
                        item.IsEnabled = reader.GetBoolean("IsEnabled");
                        item.Category = new Category()
                        {
                            ID = reader.GetInt32("Kategorija")
                        };
                        item.Retailer = new Retailer()
                        {
                            ID = reader.GetInt32("RetailerID")
                        };

                        if (Convert.ToBoolean(reader.GetValue("IsMonitoredByUser")))
                        {
                            item.Monitored = new List<MonitoredItem>();
                            item.Monitored.Add(new MonitoredItem()
                            {
                                User = new User()
                                {
                                    ID = Convert.ToInt32(parameters["userId"])
                                }
                            });
                        }

                        items.Add(item);
                    }
                }
            }

            return items;
        }


        private async Task<int> GetSearchItemsCountAsync(
                Dictionary<string, object> parameters,
                string sql, 
                CancellationToken cancellationToken
            )
        {
            int totalItemsFound = 0;

            using (var connection = _dbContextFactory.CreateDbContext().Database.GetDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sql;


                //var countItemsCommand = new SqlCommand();
                //countItemsCommand.CommandText = sql;

                var queryParameters = new List<SqlParameter>();
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        queryParameters.Add(new SqlParameter(p.Key, p.Value));
                    }
                }
                //countItemsCommand.Parameters.AddRange(queryParameters.ToArray());
                command.Parameters.AddRange(queryParameters.ToArray());

                //await connection.OpenAsync(cancellationToken);

                var reader = await command.ExecuteReaderAsync(cancellationToken);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        totalItemsFound = reader.GetInt32("TotalItemsFound");
                    }
                }
            }

            return totalItemsFound;
        }

        #endregion
    }
}
