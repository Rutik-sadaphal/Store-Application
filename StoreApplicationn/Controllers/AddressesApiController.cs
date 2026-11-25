using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/addresses")]
    public class AddressesApiController : ApiController
    {
        private readonly IAddressService addressService;

        public AddressesApiController()
        {
            addressService = new AddressService();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddAddress(AddressCreateDto request)
        {
            if (request == null)
                return BadRequest("Address details are required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var address = await addressService.AddAddress(request);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpGet]
        [Route("user/{userId:int}")]
        public async Task<IHttpActionResult> GetAddresses(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid User ID.");

            var addresses = await addressService.GetAddressesByUserId(userId);

            return Ok(addresses);
        }

        [HttpGet]
        [Route("{addressId:int}")]
        public async Task<IHttpActionResult> GetAddress(int addressId)
        {
            if (addressId <= 0)
                return BadRequest("Invalid Address ID.");

            var address = await addressService.GetAddressById(addressId);

            if (address == null)
                return NotFound();

            return Ok(address);
        }
    }
}