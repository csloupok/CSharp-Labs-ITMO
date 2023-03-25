using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private const int MinCourseNumber = 1;
    private const int MaxCourseNumber = 5;
    private int _number;

    public CourseNumber(int number)
    {
        if (number is < MinCourseNumber or > MaxCourseNumber)
            throw new IsuException("Incorrect course number!");
        _number = number;
    }

    public int Number => _number;
}