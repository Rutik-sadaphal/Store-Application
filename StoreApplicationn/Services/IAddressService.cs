using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface IAddressService
    {
        Task<AddressResponseDto> AddAddress(AddressCreateDto request);
        Task<List<AddressResponseDto>> GetAddressesByUserId(int userId);
        Task<AddressResponseDto> GetAddressById(int addressId);
    }
}