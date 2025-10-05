using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommercePlatform.Core.Entities;
public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal SubTotal => Items.Sum(i => i.Total);
    public decimal ShippingFee { get; set; }
    public decimal Total => SubTotal + ShippingFee;
    public int ItemCount => Items.Sum(i => i.Quantity);
}