using Isu.Entities;
using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Models;

public class ExtraStudent : Student
{
    private const int MaxNumberOfExtraCourses = 2;
    private ExtraGroup _group;
    private List<Stream> _streams;

    public ExtraStudent(string name, ExtraGroup group, int id)
        : base(name, null, id)
    {
        _group = group ?? throw GroupException.NullArgument();
        _streams = new List<Stream>(MaxNumberOfExtraCourses);
    }

    public new ExtraGroup Group => _group;
    public IReadOnlyList<Stream> Streams => _streams;
    public void ChangeGroup(ExtraGroup newGroup)
    {
        if (newGroup is null)
            throw GroupException.NullArgument();
        newGroup.AddStudent(this);
        _group.RemoveStudent(this);
        _group = newGroup;
    }

    public void ChooseExtraCourse(Stream stream)
    {
        if (stream.ExtraCourse is null)
            throw CourseException.NullArgument();
        if (stream.ExtraCourse.Faculty == _group.Faculty)
            throw CourseException.SameFacultyEnroll();
        if (_streams.Contains(stream))
            throw CourseException.SameCourseEnroll();
        if (_streams.Count >= MaxNumberOfExtraCourses)
            throw CourseException.TooManyCourse();
        if (_group.Schedule is null)
            throw ScheduleException.NoGroupSchedule();
        if (stream.Schedule is null)
            throw ScheduleException.NoStreamSchedule();
        if (_group.Schedule.IsConflicted(stream.Schedule.Lessons))
            throw ScheduleException.Conflict();
        _streams.Add(stream);
    }

    public void LeaveExtraCourse(Stream stream)
    {
        if (stream.ExtraCourse is null)
            throw CourseException.NullArgument();
        if (!_streams.Contains(stream))
            throw CourseException.NotEnrolled();
        _streams.Remove(stream);
    }
}