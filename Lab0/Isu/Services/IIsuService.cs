using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService<TStudent, TGroup>
    where TStudent : class
    where TGroup : class
{
    TGroup AddGroup(GroupName name);
    TStudent AddStudent(TGroup group, string name);

    TStudent GetStudent(int id);
    TStudent? FindStudent(int id);
    IReadOnlyList<TStudent>? FindStudents(GroupName groupName);
    IReadOnlyList<TStudent> FindStudents(CourseNumber courseNumber);

    TGroup? FindGroup(GroupName groupName);
    IReadOnlyList<TGroup> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(TStudent student, TGroup newGroup);
}