using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Entities;

public class Teacher
{
    private string _name;

    public Teacher(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw StringException.Empty(nameof(name));
        _name = name;
    }

    public string Name => _name;
}