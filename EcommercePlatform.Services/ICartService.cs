using EcommercePlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommercePlatform.Services;
public interface ICartService
{
    ShoppingCart GetCart();
    void AddToCart(int productId, string productName, string productImage, decimal price, int quantity = 1);
    void UpdateQuantity(int productId, int quantity);
    void RemoveFromCart(int productId);
    void ClearCart();
}
