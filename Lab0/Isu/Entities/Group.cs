using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class Group<TStudent>
    where TStudent : class
{
    public const int MaxNumberOfStudents = 30;
    private GroupName _groupName;
    private List<TStudent> _students;
    private int _courseNumber;

    public Group(GroupName groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName.Name))
            throw new IsuException("Group name is empty or null!");
        _groupName = groupName;
        _students = new List<TStudent>();
        _courseNumber = new CourseNumber(short.Parse(_groupName.Name.Substring(2, 1))).Number;
    }

    public GroupName GroupName => _groupName;
    public IReadOnlyList<TStudent> Students => _students;
    public int CourseNumber => _courseNumber;
    public override string ToString() => _groupName.Name;

    public TStudent AddStudent(TStudent student)
    {
        if (student is null)
            throw new IsuException("Student is null!");
        if (_students.Count >= MaxNumberOfStudents)
            throw new IsuException("Group is full!");

        _students.Add(student);
        return student;
    }

    public void RemoveStudent(TStudent student)
    {
        if (student is null)
            throw new IsuException("Student is null!");
        _students.Remove(student);
    }
}