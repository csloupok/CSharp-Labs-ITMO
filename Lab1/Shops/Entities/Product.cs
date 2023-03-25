using Shops.Tools.Exceptions;

namespace Shops.Entities;

public class Product
{
    private string _name;
    private int _id;

    public Product(string name, int id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductException("Product name is empty");
        _name = name;
        _id = id;
    }

    public string Name => _name;
    public int Id => _id;
}