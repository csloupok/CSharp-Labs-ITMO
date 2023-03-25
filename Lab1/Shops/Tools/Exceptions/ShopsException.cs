namespace Shops.Tools.Exceptions;

public class ShopsException : Exception
{
    public ShopsException(string message)
        : base(message) { }
}