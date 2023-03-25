using Shops.Entities;
using Shops.Models;

namespace Shops.Services;

public interface IShopManager
{
    Shop RegisterShop(string name, string address);
    Product RegisterProduct(string name);
    Customer RegisterCustomer(string name, decimal credits);
    CartProduct AddProductToCart(Product product, int amount);
    Shop? FindShop(int id);
    Customer? FindCustomer(string name);
    Shop? FindCheapestShop(List<CartProduct> cartProducts);
}