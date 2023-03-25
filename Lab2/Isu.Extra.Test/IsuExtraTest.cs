using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Extra.Tools.Exceptions;
using Isu.Models;
using Xunit;
using Stream = Isu.Extra.Models.Stream;

namespace Isu.Extra.Test;

public class IsuExtraTest
{
   private IsuExtraService _isu = new IsuExtraService();

   [Fact]
   public void CreateExtraCourseAndStreams_StreamsHaveCourseAndCourseContainsStreams()
   {
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'M');
      Stream stream1 = _isu.AddStream("Stream1", extraCourse);
      Stream stream2 = _isu.AddStream("Stream2", extraCourse);

      Assert.Same(_isu.GetStreams(extraCourse), extraCourse.Streams);
      Assert.Same(stream1.ExtraCourse, extraCourse);
      Assert.Same(stream2.ExtraCourse, extraCourse);
   }

   [Fact]
   public void AddStudentToStream_StudentHasStream_StreamHasStudent()
   {
      Room room = new Room(1);
      Teacher teacher = new Teacher("Teacher");
      Time time1 = new Time("8:20 AM", "9:50 AM");
      Time time2 = new Time("10:00 AM", "11:30 AM");

      Schedule groupSchedule = new Schedule();
      groupSchedule.AddLesson(new Lesson("Lesson1", "Monday", time1, teacher, room));
      Schedule streamSchedule = new Schedule();
      streamSchedule.AddLesson(new Lesson("Lesson2", "Monday", time2, teacher, room));

      ExtraGroup group = _isu.AddGroup(new GroupName("M32051"));
      ExtraStudent student = _isu.AddStudent(group, "Student");
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'K');
      Stream stream = _isu.AddStream("Stream", extraCourse);

      _isu.SetSchedule(group, groupSchedule);
      _isu.SetSchedule(stream, streamSchedule);
      _isu.EnrollToExtraCourse(student, stream);

      Assert.Contains(stream, student.Streams);
      Assert.Contains(student, _isu.FindStudents(stream));
   }

   [Fact]
   public void StudentUnsubscribesFromCourse_StudentIsUnsubscribedAndStreamHasNoStudent()
   {
      Room room = new Room(1);
      Teacher teacher = new Teacher("Teacher");
      Time time1 = new Time("8:20 AM", "9:50 AM");
      Time time2 = new Time("10:00 AM", "11:30 AM");

      Schedule groupSchedule = new Schedule();
      groupSchedule.AddLesson(new Lesson("Lesson1", "Monday", time1, teacher, room));
      Schedule streamSchedule = new Schedule();
      streamSchedule.AddLesson(new Lesson("Lesson2", "Monday", time2, teacher, room));

      ExtraGroup group = _isu.AddGroup(new GroupName("M32051"));
      ExtraStudent student = _isu.AddStudent(group, "Student");
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'K');
      Stream stream = _isu.AddStream("Stream", extraCourse);

      _isu.SetSchedule(group, groupSchedule);
      _isu.SetSchedule(stream, streamSchedule);
      _isu.EnrollToExtraCourse(student, stream);
      _isu.UnsubscribeFromExtraCourse(student, stream);

      Assert.DoesNotContain(student, stream.Students);
      Assert.Contains(student, _isu.FindNotEnrolledStudents(group));
   }

   [Fact]
   public void StudentEnrollAndExceptionIsThrown_SameFaculty()
   {
      Room room = new Room(1);
      Teacher teacher = new Teacher("Teacher");
      Time time1 = new Time("8:20 AM", "9:50 AM");
      Time time2 = new Time("10:00 AM", "11:30 AM");

      Schedule groupSchedule = new Schedule();
      groupSchedule.AddLesson(new Lesson("Lesson1", "Monday", time1, teacher, room));
      Schedule streamSchedule = new Schedule();
      streamSchedule.AddLesson(new Lesson("Lesson2", "Monday", time2, teacher, room));

      ExtraGroup group = _isu.AddGroup(new GroupName("M32051"));
      ExtraStudent student = _isu.AddStudent(group, "Student");
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'M');
      Stream stream = _isu.AddStream("Stream", extraCourse);

      _isu.SetSchedule(group, groupSchedule);
      _isu.SetSchedule(stream, streamSchedule);
      CourseException exception = Assert.Throws<CourseException>(() => _isu.EnrollToExtraCourse(student, stream));
      Assert.Equal("Can't enroll to same faculty course!", exception.Message);
   }

   [Fact]
   public void StudentEnrollAndExceptionIsThrown_SameCourse()
   {
      Room room = new Room(1);
      Teacher teacher = new Teacher("Teacher");
      Time time1 = new Time("8:20 AM", "9:50 AM");
      Time time2 = new Time("10:00 AM", "11:30 AM");

      Schedule groupSchedule = new Schedule();
      groupSchedule.AddLesson(new Lesson("Lesson1", "Monday", time1, teacher, room));
      Schedule streamSchedule = new Schedule();
      streamSchedule.AddLesson(new Lesson("Lesson2", "Monday", time2, teacher, room));

      ExtraGroup group = _isu.AddGroup(new GroupName("M32051"));
      ExtraStudent student = _isu.AddStudent(group, "Student");
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'K');
      Stream stream = _isu.AddStream("Stream", extraCourse);

      _isu.SetSchedule(group, groupSchedule);
      _isu.SetSchedule(stream, streamSchedule);
      _isu.EnrollToExtraCourse(student, stream);
      CourseException exception = Assert.Throws<CourseException>(() => _isu.EnrollToExtraCourse(student, stream));
      Assert.Equal("Can't enroll to same course!", exception.Message);
   }

   [Fact]
   public void StudentEnrollAndExceptionIsThrown_ConflictingSchedule()
   {
      Room room = new Room(1);
      Teacher teacher = new Teacher("Teacher");
      Time time1 = new Time("8:20 AM", "9:50 AM");
      Time time2 = new Time("8:20 AM", "9:50 AM");

      Schedule groupSchedule = new Schedule();
      groupSchedule.AddLesson(new Lesson("Lesson1", "Monday", time1, teacher, room));
      Schedule streamSchedule = new Schedule();
      streamSchedule.AddLesson(new Lesson("Lesson2", "Monday", time2, teacher, room));

      ExtraGroup group = _isu.AddGroup(new GroupName("M32051"));
      ExtraStudent student = _isu.AddStudent(group, "Student");
      ExtraCourse extraCourse = _isu.AddExtraCourse("Course", 'K');
      Stream stream = _isu.AddStream("Stream", extraCourse);

      _isu.SetSchedule(group, groupSchedule);
      _isu.SetSchedule(stream, streamSchedule);
      ScheduleException exception = Assert.Throws<ScheduleException>(() => _isu.EnrollToExtraCourse(student, stream));
      Assert.Equal("Schedule is conflicted!", exception.Message);
   }
}