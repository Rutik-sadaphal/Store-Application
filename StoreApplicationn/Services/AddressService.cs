using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly IUserRepository userRepository;

        public AddressService()
        {
            addressRepository = new AddressRepository();
            userRepository = new UserRepository();
        }

        public async Task<AddressResponseDto> AddAddress(AddressCreateDto request)
        {
            var user = await userRepository.GetById(request.UserId);

            if (user == null)
                return null;

            if (request.IsDefault)
            {
                var addresses = await addressRepository.GetAddressesByUserId(
                    request.UserId);

                foreach (var address in addresses)
                {
                    address.IsDefault = false;
                }
            }

            var newAddress = new Address
            {
                UserId = request.UserId,
                AddressLine = request.AddressLine,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                IsDefault = request.IsDefault
            };

            addressRepository.AddAddress(newAddress);
            await addressRepository.SaveChanges();

            return MapAddress(newAddress);
        }

        public async Task<List<AddressResponseDto>> GetAddressesByUserId(int userId)
        {
            var addresses = await addressRepository.GetAddressesByUserId(userId);

            return addresses.Select(a => MapAddress(a)).ToList();
        }

        public async Task<AddressResponseDto> GetAddressById(int addressId)
        {
            var address = await addressRepository.GetAddressById(addressId);

            if (address == null)
                return null;

            return MapAddress(address);
        }

        private AddressResponseDto MapAddress(Address address)
        {
            return new AddressResponseDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsDefault = address.IsDefault
            };
        }
    }
}