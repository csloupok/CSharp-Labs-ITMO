using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Models;

public class Stream
{
    private const int MaxNumberOfStudents = 15;
    private List<ExtraStudent> _students;
    private Schedule? _schedule;
    private ExtraCourse _extraCourse;
    private string _name;

    public Stream(string name, ExtraCourse extraCourse)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw StringException.Empty(nameof(name));
        _name = name;
        _extraCourse = extraCourse ?? throw CourseException.NullArgument();
        _schedule = null;
        _students = new List<ExtraStudent>(MaxNumberOfStudents);
    }

    public string Name => _name;
    public IReadOnlyList<ExtraStudent> Students => _students;
    public Schedule? Schedule => _schedule;
    public ExtraCourse ExtraCourse => _extraCourse;

    internal void AddStudent(ExtraStudent student)
    {
        if (student is null)
            throw StudentException.NullArgument();
        if (_students.Count >= MaxNumberOfStudents)
            throw StreamException.Full();

        student.ChooseExtraCourse(this);
        _students.Add(student);
    }

    internal void RemoveStudent(ExtraStudent student)
    {
        if (student is null)
            throw StudentException.NullArgument();

        student.LeaveExtraCourse(this);
        _students.Remove(student);
    }

    internal void SetSchedule(Schedule schedule)
    {
        _schedule = schedule ?? throw ScheduleException.NullArgument();
    }
}