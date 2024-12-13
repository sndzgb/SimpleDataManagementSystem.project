using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class RetailersCoreService : IRetailersCoreService
    {
        private readonly IRetailersCoreRepository _retailersCoreRepository;


        public RetailersCoreService(IRetailersCoreRepository retailersCoreRepository)
        {
            _retailersCoreRepository = retailersCoreRepository;
        }


        public async Task<CreateRetailerResponseDTO> CreateRetailerAsync(CreateRetailerRequestDTO createRetailerRequestDTO, CancellationToken cancellationToken)
        {
            var createdRetailer = await _retailersCoreRepository.CreateRetailerAsync(createRetailerRequestDTO, cancellationToken);

            var response = new CreateRetailerResponseDTO()
            {
                Id = createdRetailer.ID,
                LogoImageUrl = createdRetailer.LogoImageUrl,
                Name = createdRetailer.Name,
                Priority = createdRetailer.Priority
            };

            return response;
        }

        public async Task DeleteRetailerAsync(DeleteRetailerRequestDTO deleteRetailerRequestDTO, CancellationToken cancellationToken)
        {
            await _retailersCoreRepository.DeleteRetailerAsync(deleteRetailerRequestDTO, cancellationToken);
        }

        public async Task<GetRetailerResponseDTO?> GetRetailerAsync(GetRetailerRequestDTO getRetailerRequestDTO, CancellationToken cancellationToken)
        {
            var retailer = await _retailersCoreRepository.GetRetailerAsync(getRetailerRequestDTO, cancellationToken);

            if (retailer == null)
            {
                return null;
            }

            var response = new GetRetailerResponseDTO()
            {
                Id = retailer.ID,
                LogoImageUrl = retailer.LogoImageUrl,
                Priority = retailer.Priority,
                Name = retailer.Name
            };

            return response;
        }

        public async Task<GetRetailersResponseDTO?> GetRetailersAsync(GetRetailersRequestDTO getRetailersRequestDTO, CancellationToken cancellationToken)
        {
            var retailers = await _retailersCoreRepository.GetRetailersAsync(getRetailersRequestDTO, cancellationToken);

            GetRetailersResponseDTO? response = new GetRetailersResponseDTO();

            if (retailers != null)
            {
                if (retailers.Retailers != null && retailers.Retailers.Count() > 0)
                {
                    response = new GetRetailersResponseDTO();
                    response.Retailers = new List<GetRetailersResponseDTO.RetailerDTO>();

                    foreach (var retailer in retailers.Retailers)
                    {
                        response.Retailers.Add(new GetRetailersResponseDTO.RetailerDTO()
                        {
                            Id = retailer.ID,
                            LogoImageUrl = retailer.LogoImageUrl,
                            Name = retailer.Name,
                            Priority = retailer.Priority
                        });
                    }
                }
            }

            response.PageInfo = new GetRetailersResponseDTO.PageDTO()
            {
                Total = retailers.PageInfo.Total
            };

            return response;
        }

        public async Task UpdateRetailerAsync(UpdateRetailerRequestDTO updateRetailerRequestDTO, CancellationToken cancellationToken)
        {
            if (updateRetailerRequestDTO == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(updateRetailerRequestDTO));
            }

            var existingRetailer = _retailersCoreRepository.GetRetailerAsync(new GetRetailerRequestDTO()
            {
                RequestedByUserId = updateRetailerRequestDTO.RequestedByUserId,
                RetailerId = updateRetailerRequestDTO.RequestMetadata.RetailerId
            }, cancellationToken);

            if (existingRetailer == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _retailersCoreRepository.UpdateRetailerAsync(updateRetailerRequestDTO, cancellationToken);
        }
    }
}
