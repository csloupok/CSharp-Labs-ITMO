using Shops.Models;
using Shops.Tools.Exceptions;

namespace Shops.Entities;

public class Shop
{
    private const int MinimalAmountOfCredits = 0;
    private string _name;
    private int _id;
    private string _address;
    private List<ShopProduct> _products;
    private decimal _credits;

    public Shop(string name, int id, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ShopsException("Shop name can't be empty");
        if (string.IsNullOrWhiteSpace(address))
            throw new ShopsException("Shop address can't be empty");
        _name = name;
        _products = new List<ShopProduct>();
        _id = id;
        _address = address;
        _credits = MinimalAmountOfCredits;
    }

    public string Name => _name;
    public IReadOnlyList<ShopProduct> Products => _products;
    public int Id => _id;
    public string Address => _address;
    public decimal Credits => _credits;

    public ShopProduct AddProduct(Product product)
    {
        ShopProduct shopProduct = new ShopProduct(product);
        if (_products.Any(x => x.Product.Id == product.Id))
            throw new ProductException($"{shopProduct.Product.Name} is already in store");
        _products.Add(shopProduct);
        return shopProduct;
    }

    public bool IsInStock(Product product)
    {
        return _products.Any(x => x.Product.Id == product.Id);
    }

    public bool IsInStock(List<CartProduct> cartProducts)
    {
        return cartProducts.All(cartProduct =>
            _products.Any(x => x.Product.Id == cartProduct.Product.Id && x.Quantity >= cartProduct.AmountToBuy));
    }

    public ShopProduct GetProduct(Product product)
    {
        if (!IsInStock(product))
            throw new ProductException($"Store doesn't contain {product.Name}");
        ShopProduct shopProduct = _products.Single(x => x.Product.Id == product.Id);
        return shopProduct;
    }

    public void RefillStock(Product product, int amount)
    {
        if (!IsInStock(product))
            throw new ProductException($"Store doesn't contain {product.Name}");
        if (product is null)
            throw new ProductException("Product is null");

        ShopProduct shopProduct = _products.Single(x => x.Product.Id == product.Id);
        shopProduct.ChangeQuantity(shopProduct.Quantity + amount);
    }

    public decimal Buy(CartProduct cartProduct, Customer customer, int amount = 1)
    {
        if (!IsInStock(cartProduct.Product))
            throw new ProductException($"Store doesn't contain {cartProduct.Product.Name}");

        ShopProduct shopProduct = _products.Single(x => x.Product.Id == cartProduct.Product.Id);

        if (shopProduct.Quantity <= cartProduct.AmountToBuy)
            throw new ProductException($"{shopProduct.Product.Name} in quantity of {cartProduct.AmountToBuy} is not in stock");
        if (shopProduct.Price * cartProduct.AmountToBuy > customer.Credits)
            throw new CreditsException($"{customer.Name} doesn't have sufficient credits. \n Total is {shopProduct.Price * cartProduct.AmountToBuy}. Customer's credits are {customer.Credits}");

        decimal total = shopProduct.Price * cartProduct.AmountToBuy;
        shopProduct.ChangeQuantity(shopProduct.Quantity - cartProduct.AmountToBuy);
        customer.WithdrawCredits(total);
        return DepositCredits(total);
    }

    public decimal Buy(List<CartProduct> cartProducts, Customer customer)
    {
        if (!IsInStock(cartProducts))
            throw new ProductException("Not enough products in store");
        decimal total = 0;
        foreach (CartProduct cartProduct in cartProducts)
        {
            ShopProduct shopProduct = _products.Single(x => x.Product.Id == cartProduct.Product.Id);
            total += shopProduct.Price * cartProduct.AmountToBuy;
        }

        if (total > customer.Credits)
        {
            throw new CreditsException(
                $"{customer.Name} doesn't have sufficient credits. \n Total is {total}. Customer's credits are {customer.Credits}");
        }

        foreach (CartProduct cartProduct in cartProducts)
            Buy(cartProduct, customer);

        return total;
    }

    public void ChangeProductPrice(Product product, decimal price)
    {
        if (!IsInStock(product))
            throw new ProductException($"Store doesn't contain {product.Name}");
        ShopProduct shopProduct = _products.Single(x => x.Product.Id == product.Id);
        shopProduct.ChangePrice(price);
    }

    private decimal DepositCredits(decimal value)
    {
        _credits += value;
        return value;
    }
}