using EcommercePlatform.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EcommercePlatform.Services;
public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "ShoppingCart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ShoppingCart GetCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cartBytes = session.TryGetValue(CartSessionKey, out var value) ? value : null;
        var cartJson = cartBytes != null ? System.Text.Encoding.UTF8.GetString(cartBytes) : null;

        if (string.IsNullOrEmpty(cartJson))
        {
            return new ShoppingCart();
        }

        return JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();
    }

    public void AddToCart(int productId, string productName, string productImage, decimal price, int quantity = 1)
    {
        var cart = GetCart();
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                ProductName = productName,
                ProductImage = productImage,
                Price = price,
                Quantity = quantity
            });
        }

        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCart(cart);
        }
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cart.Items.Remove(item);
            SaveCart(cart);
        }
    }

    public void ClearCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.Remove(CartSessionKey);
    }

    private void SaveCart(ShoppingCart cart)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cartJson = JsonSerializer.Serialize(cart);
        session.Set(CartSessionKey, System.Text.Encoding.UTF8.GetBytes(cartJson));
    }
}