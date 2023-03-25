using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;
using Stream = Isu.Extra.Models.Stream;

namespace Isu.Extra.Services;

public interface IIsuExtraService : IIsuService<ExtraStudent, ExtraGroup>
{
    ExtraCourse AddExtraCourse(string courseName, char faculty);
    Stream AddStream(string name, ExtraCourse extraCourse);
    void SetSchedule(ExtraGroup group, Schedule schedule);
    void SetSchedule(Stream stream, Schedule schedule);

    IReadOnlyList<ExtraStudent> FindStudents(Stream stream);
    IReadOnlyList<Stream> GetStreams(ExtraCourse extraCourse);
    IReadOnlyList<ExtraStudent> FindNotEnrolledStudents(ExtraGroup group);

    void EnrollToExtraCourse(ExtraStudent student, Stream stream);
    void UnsubscribeFromExtraCourse(ExtraStudent student, Stream stream);
}