using System.Runtime.CompilerServices;
using Shops.Entities;
using Shops.Models;
using Shops.Tools;
using Shops.Tools.Exceptions;

namespace Shops.Services;

public class ShopManager : IShopManager
{
    private readonly IdGenerator _shopIdGenerator = new IdGenerator();
    private readonly IdGenerator _productIdGenerator = new IdGenerator();
    private List<Shop> _allShops = new List<Shop>();
    private List<Customer> _allCustomers = new List<Customer>();

    public Shop RegisterShop(string name, string address)
    {
        if (IsAddressBusy(address))
            throw new ShopsException("Address is busy");
        Shop shop = new Shop(name, _shopIdGenerator.GenerateId(), address);
        _allShops.Add(shop);
        return shop;
    }

    public Product RegisterProduct(string name)
    {
        Product product = new Product(name, _productIdGenerator.GenerateId());
        return product;
    }

    public Customer RegisterCustomer(string name, decimal credits)
    {
        Customer customer = new Customer(name, credits);
        _allCustomers.Add(customer);
        return customer;
    }

    public CartProduct AddProductToCart(Product product, int amount = 1)
    {
        return new CartProduct(product, amount);
    }

    public Shop? FindShop(int id)
    {
        return _allShops.Find(shop => shop.Id == id);
    }

    public Customer? FindCustomer(string name)
    {
        return _allCustomers.Find(customer => customer.Name == name);
    }

    public Shop? FindCheapestShop(List<CartProduct> cartProducts)
    {
        List<Product> products = new List<Product>();
        foreach (CartProduct cartProduct in cartProducts)
        {
            products.Add(cartProduct.Product);
        }

        if (cartProducts is null)
            throw new ProductException("List of products is empty or null");
        List<Shop> shopsWithProductsInStock = _allShops.Where(shop => shop.IsInStock(cartProducts)).ToList();
        decimal minTotal = decimal.MaxValue;
        if (shopsWithProductsInStock.Count == 0)
            throw new ShopsException("Currently no shop has these products in stock");
        Shop? cheapestShop = null;
        foreach (Shop shop in shopsWithProductsInStock)
        {
            decimal total = products.Sum(product => shop.Products.Single(x => x.Product.Id == product.Id).Price);
            if (total < minTotal)
            {
                minTotal = total;
                cheapestShop = shop;
            }
        }

        return cheapestShop;
    }

    private bool IsAddressBusy(string address)
    {
        return _allShops.Any(shop => shop.Address == address);
    }
}