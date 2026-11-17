using System;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public class CartCalculationService
    {
        private const decimal TaxPercentage = 18m;
        private const decimal ShippingCharge = 50m;

        public void Calculate(CartResponseDto cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            cart.Tax = Math.Round(
                cart.Subtotal * TaxPercentage / 100m, 2);

            cart.ShippingCost = ShippingCharge;

            cart.GrandTotal =
                cart.Subtotal + cart.Tax + cart.ShippingCost;
        }
    }
}