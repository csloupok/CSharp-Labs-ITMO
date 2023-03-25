using Shops.Entities;
using Shops.Tools.Exceptions;

namespace Shops.Models;

public class ShopProduct
{
    private const int MinimalPrice = 0;
    private const int MinimalQuantity = 0;
    private Product _product;
    private decimal _price;
    private int _quantity;

    public ShopProduct(Product product)
    {
        _product = product ?? throw new ProductException("Product is null");
        _price = MinimalPrice;
        _quantity = MinimalQuantity;
    }

    public Product Product => _product;
    public decimal Price => _price;
    public int Quantity => _quantity;

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < MinimalPrice)
            throw new CreditsException("Price can't be negative");
        _price = newPrice;
    }

    public void ChangeQuantity(int quantity)
    {
        if (quantity < MinimalQuantity)
            throw new CreditsException("Quantity can't be negative");
        _quantity = quantity;
    }
}