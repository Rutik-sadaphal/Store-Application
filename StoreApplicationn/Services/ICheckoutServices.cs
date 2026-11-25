using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface ICheckoutServices
    {
        Task<string> ValidateCustomerDetails(int userId);
        Task<string> ValidateBillingAddress(int userId, int billingAddressId);

        Task<string> ValidateShippingAddress(int userId, int shippingAddressId);

        Task<string> ValidateCart(int userId);

        Task<string> ValidateProducts(int userId);

        Task<CheckoutResponseDto> ValidateCheckout(
            int userId,
            int billingAddressId,
            int shippingAddressId); 
    }
}
