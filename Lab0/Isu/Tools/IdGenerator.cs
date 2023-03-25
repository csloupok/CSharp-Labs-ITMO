namespace Isu.Tools;

public class IdGenerator
{
    private int _id = 100000;

    public int GenerateId()
    {
       _id++;
       return _id;
    }
}