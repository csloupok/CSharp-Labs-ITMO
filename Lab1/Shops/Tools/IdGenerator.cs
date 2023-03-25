namespace Shops.Tools;

public class IdGenerator
{
    private int _id;

    public int GenerateId()
    {
        _id++;
        return _id;
    }
}