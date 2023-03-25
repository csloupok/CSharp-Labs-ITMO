using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services;

public class IsuService : IIsuService<Student, Group<Student>>
{
    private List<Group<Student>> _allGroups = new List<Group<Student>>();
    private List<Student> _allStudents = new List<Student>();
    private IdGenerator _generator = new IdGenerator();

    public Group<Student> AddGroup(GroupName name)
    {
        if (name is null)
            throw new IsuException("Group name is null!");
        if (FindGroup(name) is not null)
            throw new IsuException("Group already exists!");
        Group<Student> group = new Group<Student>(name);
        _allGroups.Add(group);
        return group;
    }

    public Student AddStudent(Group<Student> group, string name)
    {
        if (group is null)
            throw new IsuException("Group doesn't exist!");
        Student student = new Student(name, group, _generator.GenerateId());
        _allStudents.Add(student);
        return group.AddStudent(student);
    }

    public Student GetStudent(int id)
    {
        return FindStudent(id) ?? throw new IsuException("Student doesn't exist!");
    }

    public Student? FindStudent(int id)
    {
        return _allStudents.Find(student => student.Id == id);
    }

    public IReadOnlyList<Student>? FindStudents(GroupName groupName)
    {
        Group<Student>? group = FindGroup(groupName);
        return group?.Students;
    }

    public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber)
    {
        return _allStudents.FindAll(student => student.Group?.CourseNumber == courseNumber.Number);
    }

    public Group<Student>? FindGroup(GroupName groupName)
    {
        return _allGroups.Find(group => group.GroupName.Name == groupName.Name);
    }

    public IReadOnlyList<Group<Student>> FindGroups(CourseNumber courseNumber)
    {
        return _allGroups.FindAll(group => group.CourseNumber == courseNumber.Number);
    }

    public void ChangeStudentGroup(Student student, Group<Student> newGroup)
    {
        student.ChangeGroup(newGroup);
    }
}