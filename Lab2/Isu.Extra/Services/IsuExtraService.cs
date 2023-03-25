using Isu.Extra.Models;
using Isu.Extra.Tools.Exceptions;
using Isu.Models;
using Isu.Tools;
using Stream = Isu.Extra.Models.Stream;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private List<ExtraGroup> _allGroups = new List<ExtraGroup>();
    private List<ExtraStudent> _allStudents = new List<ExtraStudent>();
    private IdGenerator _generator = new IdGenerator();
    private List<ExtraCourse> _allExtraCourses = new List<ExtraCourse>();
    private List<Stream> _allStreams = new List<Stream>();

    public ExtraGroup AddGroup(GroupName name)
    {
        if (name is null)
            throw StringException.Empty(nameof(name));
        if (FindGroup(name) is not null)
            throw GroupException.AlreadyExists();
        ExtraGroup group = new ExtraGroup(name);
        _allGroups.Add(group);
        return group;
    }

    public ExtraStudent AddStudent(ExtraGroup group, string name)
    {
        if (group is null)
            throw GroupException.NullArgument();
        ExtraStudent student = new ExtraStudent(name, group, _generator.GenerateId());
        _allStudents.Add(student);
        return group.AddStudent(student);
    }

    public ExtraStudent GetStudent(int id)
    {
        return FindStudent(id) ?? throw StudentException.NullArgument();
    }

    public ExtraStudent? FindStudent(int id)
    {
        return _allStudents.Find(student => student.Id == id);
    }

    public IReadOnlyList<ExtraStudent>? FindStudents(GroupName groupName)
    {
        ExtraGroup? group = FindGroup(groupName);
        return group?.Students;
    }

    public IReadOnlyList<ExtraStudent> FindStudents(CourseNumber courseNumber)
    {
        return _allStudents.FindAll(student => student.Group.CourseNumber == courseNumber.Number);
    }

    public ExtraGroup? FindGroup(GroupName groupName)
    {
        return _allGroups.Find(group => group.GroupName.Name == groupName.Name);
    }

    public IReadOnlyList<ExtraGroup> FindGroups(CourseNumber courseNumber)
    {
        return _allGroups.FindAll(group => group.CourseNumber == courseNumber.Number);
    }

    public void ChangeStudentGroup(ExtraStudent student, ExtraGroup newGroup)
    {
        student.ChangeGroup(newGroup);
    }

    public ExtraCourse AddExtraCourse(string courseName, char faculty)
    {
        if (string.IsNullOrWhiteSpace(courseName))
            throw StringException.Empty(nameof(courseName));
        if (_allExtraCourses.Any(x => x.Faculty == faculty))
            throw CourseException.AlreadyExists();
        ExtraCourse extraCourse = new ExtraCourse(courseName, faculty);
        _allExtraCourses.Add(extraCourse);
        return extraCourse;
    }

    public Stream AddStream(string name, ExtraCourse extraCourse)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw StringException.Empty(nameof(name));
        if (extraCourse is null)
            throw CourseException.NullArgument();
        if (DoesStreamExist(name))
            throw StreamException.AlreadyExists();
        Stream stream = extraCourse.AddStream(new Stream(name, extraCourse));
        _allStreams.Add(stream);
        return stream;
    }

    public void SetSchedule(ExtraGroup group, Schedule schedule)
    {
        if (group is null)
            throw GroupException.NullArgument();
        if (schedule is null)
            throw ScheduleException.NullArgument();
        group.SetSchedule(schedule);
    }

    public void SetSchedule(Stream stream, Schedule schedule)
    {
        if (stream is null)
            throw StreamException.NullArgument();
        if (schedule is null)
            throw ScheduleException.NullArgument();
        stream.SetSchedule(schedule);
    }

    public IReadOnlyList<ExtraStudent> FindStudents(Stream stream)
    {
        return stream.Students;
    }

    public IReadOnlyList<Stream> GetStreams(ExtraCourse extraCourse)
    {
        if (extraCourse.Streams.Count == 0)
            throw CourseException.NoStreams();
        return extraCourse.Streams;
    }

    public IReadOnlyList<ExtraStudent> FindNotEnrolledStudents()
    {
        return _allStudents.FindAll(x => x.Streams.Count == 0);
    }

    public IReadOnlyList<ExtraStudent> FindNotEnrolledStudents(ExtraGroup group)
    {
        return _allStudents.FindAll(x => x.Streams.Count == 0 && x.Group == group);
    }

    public void EnrollToExtraCourse(ExtraStudent student, Stream stream)
    {
        if (student is null)
            throw StudentException.NullArgument();
        if (stream is null)
            throw StreamException.NullArgument();
        stream.AddStudent(student);
    }

    public void UnsubscribeFromExtraCourse(ExtraStudent student, Stream stream)
    {
        if (student is null)
            throw StudentException.NullArgument();
        if (stream is null)
            throw StreamException.NullArgument();
        stream.RemoveStudent(student);
    }

    private bool DoesStreamExist(string name)
    {
        return _allStreams.Any(x => x.Name == name);
    }
}