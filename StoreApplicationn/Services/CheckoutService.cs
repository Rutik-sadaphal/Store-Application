using System.Threading.Tasks;
using StoreApplicationn.Repositories;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public class CheckoutService : ICheckoutServices
    {
        private readonly IAddressRepository addressRepository;
        private readonly ICartRepository cartRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        public CheckoutService()
        {
            addressRepository = new AddressRepository();
            cartRepository = new CartRepository();
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
        }
        public async Task<string> ValidateCustomerDetails(int userId)
        {
            var user = await userRepository.GetById(userId);

            if (user == null)
                return "Customer does not exist.";

            if (string.IsNullOrWhiteSpace(user.FullName))
                return "Customer name is required.";

            if (string.IsNullOrWhiteSpace(user.Email))
                return "Customer email is required.";

            if (string.IsNullOrWhiteSpace(user.Phone))
                return "Customer phone number is required.";

            return null;
        }

        public async Task<string> ValidateBillingAddress(
            int userId, int billingAddressId)
        {
            bool isValid = await addressRepository.AddressBelongsToUser(
                billingAddressId, userId);

            if (!isValid)
                return "Invalid billing address.";

            return null;
        }

        public async Task<string> ValidateShippingAddress(
           int userId, int shippingAddressId)
        {
            bool isValid = await addressRepository.AddressBelongsToUser(
                shippingAddressId, userId);

            if (!isValid)
                return "Invalid shipping address.";

            return null;
        }


        public async Task<string> ValidateCart(int userId)
        {
            var cart = await cartRepository.GetCartResponse(userId);

            if (cart == null || cart.Items == null || cart.Items.Count == 0)
                return "Your cart is empty.";

            return null;
        }


        public async Task<string> ValidateProducts(int userId)
        {
            var cart = await cartRepository.GetCartResponse(userId);

            if (cart == null || cart.Items == null || cart.Items.Count == 0)
                return "Your cart is empty.";

            foreach (var item in cart.Items)
            {
           
                var product = await productRepository.GetProductById(
                    item.ProductId);

                if (product == null)
                    return $"Product with ID {item.ProductId} does not exist.";

                if (!product.IsActive)
                    return $"Product '{product.ProductName}' is currently unavailable.";

                
                var stockProduct = await cartRepository.GetProductById(
                    item.ProductId);

                if (stockProduct == null)
                    return $"Product with ID {item.ProductId} does not exist.";

                if (stockProduct.StockQuantity < item.Quantity)
                {
                    return $"Insufficient stock for '{product.ProductName}'. " +
                           $"Available: {stockProduct.StockQuantity}, " +
                           $"Requested: {item.Quantity}.";
                }
            }

            return null;
        }



        public async Task<CheckoutResponseDto> ValidateCheckout(
            int userId,
            int billingAddressId,
            int shippingAddressId)
        {
            string result;

            result = await ValidateCustomerDetails(userId);
            if (result != null)
            {
                return new CheckoutResponseDto
                {
                    Success = false,
                    Message = result
                };
            }

            result = await ValidateBillingAddress(userId, billingAddressId);
            if (result != null)
            {
                return new CheckoutResponseDto
                {
                    Success = false,
                    Message = result
                };
            }

            result = await ValidateShippingAddress(userId, shippingAddressId);
            if (result != null)
            {
                return new CheckoutResponseDto
                {
                    Success = false,
                    Message = result
                };
            }

            result = await ValidateCart(userId);
            if (result != null)
            {
                return new CheckoutResponseDto
                {
                    Success = false,
                    Message = result
                };
            }

            result = await ValidateProducts(userId);
            if (result != null)
            {
                return new CheckoutResponseDto
                {
                    Success = false,
                    Message = result
                };
            }

            var cart = await cartRepository.GetCartResponse(userId);

            decimal subtotal = 0;

            foreach (var item in cart.Items)
            {
                var product = await productRepository.GetProductById(item.ProductId);

                subtotal += product.Price * item.Quantity;
            }

            decimal tax = System.Math.Round(subtotal * 18m / 100m, 2);
            decimal shippingCost = 50m;

            return new CheckoutResponseDto
            {
                Success = true,
                Message = "Checkout validation successful.",
                UserId = userId,
                BillingAddressId = billingAddressId,
                ShippingAddressId = shippingAddressId,
                CartItemCount = cart.Items.Count,
                Subtotal = subtotal,
                Tax = tax,
                ShippingCost = shippingCost,
                GrandTotal = subtotal + tax + shippingCost
            };

        }
        
    }

          

}
    