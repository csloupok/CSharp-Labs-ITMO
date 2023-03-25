using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Models;

public class Schedule
{
    private List<Lesson> _lessons;

    public Schedule()
    {
        _lessons = new List<Lesson>();
    }

    public Schedule(List<Lesson> lessons)
    {
        _lessons = lessons ?? throw LessonException.NullArgument(nameof(lessons));
    }

    public IReadOnlyList<Lesson> Lessons => _lessons;

    public Lesson AddLesson(Lesson lesson)
    {
        if (lesson is null)
            throw LessonException.NullArgument(nameof(lesson));
        if (IsConflicted(lesson))
            throw LessonException.Conflict();

        _lessons.Add(lesson);
        return lesson;
    }

    public void RemoveLesson(Lesson lesson)
    {
        if (lesson is null)
            throw LessonException.NullArgument(nameof(lesson));
        if (!_lessons.Contains(lesson))
            throw LessonException.NoLesson();
        _lessons.Remove(lesson);
    }

    public bool IsConflicted(IReadOnlyList<Lesson> lessons)
    {
        if (lessons is null)
            throw LessonException.NullArgument(nameof(lessons));

        return _lessons.Any(lesson =>
            lessons.Any(x => x.Day == lesson.Day && x.Time.StartTime == lesson.Time.StartTime));
    }

    private bool IsConflicted(Lesson lesson)
    {
        if (lesson is null)
            throw LessonException.NullArgument(nameof(lesson));

        return _lessons.Any(x => x.Day == lesson.Day && x.Time.StartTime == lesson.Time.StartTime);
    }
}