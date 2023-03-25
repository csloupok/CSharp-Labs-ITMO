using System.Runtime.InteropServices;
using Shops.Entities;
using Shops.Tools.Exceptions;

namespace Shops.Models;

public class CartProduct
{
    private const int MinimalAmountToBuy = 0;
    private Product _product;
    private int _amountToBuy;

    public CartProduct(Product product, int amount = 1)
    {
        if (amount < MinimalAmountToBuy)
            throw new ProductException("Amount to buy can't be negative");
        _product = product ?? throw new ProductException("Product is null");
        _amountToBuy = amount;
    }

    public Product Product => _product;
    public int AmountToBuy => _amountToBuy;

    public void SetAmountToBuy(int amount)
    {
        if (amount < MinimalAmountToBuy)
            throw new ProductException("Amount to buy can't be negative");
        _amountToBuy = amount;
    }
}