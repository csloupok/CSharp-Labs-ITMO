using Isu.Extra.Entities;
using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Models;

public class Lesson
{
    private string _name;
    private string _day;
    private Time _time;
    private Teacher _teacher;
    private Room _room;

    public Lesson(string name, string day, Time time, Teacher teacher, Room room)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        _name = name;
        _day = day ?? throw LessonException.NullArgument(nameof(day));
        _time = time ?? throw LessonException.NullArgument(nameof(time));
        _teacher = teacher ?? throw LessonException.NullArgument(nameof(teacher));
        _room = room ?? throw LessonException.NullArgument(nameof(room));
    }

    public string Name => _name;
    public string Day => _day;
    public Time Time => _time;
    public Teacher Teacher => _teacher;
    public Room Room => _room;
}