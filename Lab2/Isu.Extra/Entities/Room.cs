using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Entities;

public class Room
{
    private const int MinNumberOfRoom = 1;
    private int _number;

    public Room(int number)
    {
        if (number < MinNumberOfRoom)
            throw new ArgumentOutOfRangeException("Wrong room number!");
        _number = number;
    }

    public int Number => _number;
}