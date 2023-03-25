using Isu.Tools;

namespace Isu.Entities;

public class Student
{
    private int _id;
    private string _name;
    private Group<Student>? _group;

    public Student(string name, Group<Student>? group, int id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("Name is empty or null!");
        _name = name;
        _group = group;
        _id = id;
    }

    public int Id => _id;
    public string Name => _name;
    public Group<Student>? Group => _group;

    public override string ToString()
    {
        return _id + " " + _name + " " + _group;
    }

    public void ChangeGroup(Group<Student> newGroup)
    {
        if (newGroup is null)
            throw new IsuException("Group is null!");
        newGroup.AddStudent(this);
        _group?.RemoveStudent(this);
        _group = newGroup;
    }
}