using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _isu = new IsuService();

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        Group<Student> group = _isu.AddGroup(new GroupName("M32092"));
        Student student = _isu.AddStudent(group, "Eldar Kasymov");
        Assert.Same(student.Group, group);
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        Group<Student> group = _isu.AddGroup(new GroupName("M32091"));
        for (int i = 0; i < Group<Student>.MaxNumberOfStudents; i++)
        {
            _isu.AddStudent(group, "Eldar Kasymov");
        }

        Assert.Throws<IsuException>(() => _isu.AddStudent(group, "Pipisa Bot"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<IsuException>(() => _isu.AddGroup(new GroupName("M32M23")));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        Group<Student> group1 = _isu.AddGroup(new GroupName("M32092"));
        Group<Student> group2 = _isu.AddGroup(new GroupName("M32051"));
        Student student = _isu.AddStudent(group1, "Eldar Kasymov");
        Assert.Same(student.Group, group1);
        Assert.Contains(student, group1.Students);
        _isu.ChangeStudentGroup(student, group2);
        Assert.Same(student.Group, group2);
        Assert.Contains(student, group2.Students);
        Assert.DoesNotContain(student, group1.Students);
    }
}