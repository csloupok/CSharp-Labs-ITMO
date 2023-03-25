using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopsTest
{
    private ShopManager _manager = new ShopManager();

    [Fact]
    public void ProductRefillTest()
    {
        const int startCredits = 1337;
        const int productPrice = 100;
        const int amountOfProductInStock = 100;
        const int amountOfProductToBuy = 10;

        Shop testShop = _manager.RegisterShop("TestShop", "TestAddress");
        Customer testCustomer = _manager.RegisterCustomer("Test Customer", startCredits);
        Product testProduct = _manager.RegisterProduct("TestProduct");
        ShopProduct testShopProduct = testShop.AddProduct(testProduct);

        testShop.RefillStock(testProduct, amountOfProductInStock);
        testShop.ChangeProductPrice(testProduct, productPrice);
        CartProduct testCartProduct = _manager.AddProductToCart(testProduct);
        testCartProduct.SetAmountToBuy(amountOfProductToBuy);
        decimal total = testShop.Buy(testCartProduct, testCustomer, amountOfProductToBuy);

        Assert.Equal(testCustomer.Credits, startCredits - total);
        Assert.Equal(testShop.Credits, total);
        Assert.Equal(testShopProduct.Quantity, amountOfProductInStock - amountOfProductToBuy);
    }

    [Fact]
    public void SetAndChangePriceTest()
    {
        const int price = 100;

        Shop testShop = _manager.RegisterShop("TestShop", "TestAddress");
        Product testProduct = _manager.RegisterProduct("TestProduct");

        testShop.AddProduct(testProduct);
        testShop.ChangeProductPrice(testProduct, price);

        Assert.Equal(price, testShop.GetProduct(testProduct).Price);
    }

    [Fact]
    public void FindShopWithLowestPriceOfOrder()
    {
        Shop testShop1 = _manager.RegisterShop("TestShop1", "TestAddress1");
        Shop testShop2 = _manager.RegisterShop("TestShop2", "TestAddress2");
        Shop testShop3 = _manager.RegisterShop("TestShop3", "TestAddress3");
        Product testProduct1 = _manager.RegisterProduct("TestProduct1");
        Product testProduct2 = _manager.RegisterProduct("TestProduct2");
        CartProduct testCartProduct1 = _manager.AddProductToCart(testProduct1);
        CartProduct testCartProduct2 = _manager.AddProductToCart(testProduct2);

        testCartProduct1.SetAmountToBuy(10);
        testCartProduct2.SetAmountToBuy(10);
        List<CartProduct> testCartProducts = new List<CartProduct>();
        testCartProducts.Add(testCartProduct1);
        testCartProducts.Add(testCartProduct2);

        testShop1.AddProduct(testProduct1);
        testShop1.AddProduct(testProduct2);
        testShop1.RefillStock(testProduct1, 10);
        testShop1.RefillStock(testProduct2, 10);
        testShop1.ChangeProductPrice(testProduct1, 100);
        testShop1.ChangeProductPrice(testProduct2, 200);

        testShop2.AddProduct(testProduct1);
        testShop2.AddProduct(testProduct2);
        testShop2.ChangeProductPrice(testProduct1, 10);
        testShop2.ChangeProductPrice(testProduct2, 20);
        testShop2.RefillStock(testProduct1, 10);
        testShop2.RefillStock(testProduct2, 10);

        testShop3.AddProduct(testProduct1);
        testShop3.AddProduct(testProduct2);
        testShop3.ChangeProductPrice(testProduct1, 1);
        testShop3.ChangeProductPrice(testProduct2, 2);

        Shop? cheapestShop = _manager.FindCheapestShop(testCartProducts);

        Assert.Equal(testShop2, cheapestShop);
    }

    [Fact]
    public void BuyListOfProducts()
    {
        const int startCredits = 1337;
        const int productPrice1 = 100;
        const int productPrice2 = 200;
        const int amountOfProductInStock = 100;
        const int amountOfProductToBuy = 3;

        Customer testCustomer = _manager.RegisterCustomer("Test Customer", startCredits);
        Shop testShop = _manager.RegisterShop("TestShop", "TestAddress");
        Product testProduct1 = _manager.RegisterProduct("TestProduct1");
        Product testProduct2 = _manager.RegisterProduct("TestProduct2");
        CartProduct testCartProduct1 = _manager.AddProductToCart(testProduct1);
        CartProduct testCartProduct2 = _manager.AddProductToCart(testProduct2);

        testCartProduct1.SetAmountToBuy(amountOfProductToBuy);
        testCartProduct2.SetAmountToBuy(amountOfProductToBuy);
        List<CartProduct> testCartProducts = new List<CartProduct>();
        testCartProducts.Add(testCartProduct1);
        testCartProducts.Add(testCartProduct2);

        testShop.AddProduct(testProduct1);
        testShop.AddProduct(testProduct2);
        testShop.ChangeProductPrice(testProduct1, productPrice1);
        testShop.ChangeProductPrice(testProduct2, productPrice2);
        testShop.RefillStock(testProduct1, amountOfProductInStock);
        testShop.RefillStock(testProduct2, amountOfProductInStock);
        decimal total = testShop.Buy(testCartProducts, testCustomer);

        Assert.Equal(testCustomer.Credits, startCredits - total);
        Assert.Equal(testShop.GetProduct(testProduct1).Quantity, amountOfProductInStock - amountOfProductToBuy);
        Assert.Equal(testShop.GetProduct(testProduct2).Quantity, amountOfProductInStock - amountOfProductToBuy);
        Assert.Equal(testShop.Credits, total);
    }
}