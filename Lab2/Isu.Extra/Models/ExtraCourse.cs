using Isu.Extra.Tools.Exceptions;

namespace Isu.Extra.Models;

public class ExtraCourse
{
    private const int MaxNumberOfStreams = 10;
    private string _name;
    private char _faculty;
    private List<Stream> _streams;

    public ExtraCourse(string name, char faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw StringException.Empty(nameof(name));
        if (!char.IsLetter(faculty))
            throw StringException.Empty(nameof(faculty));
        _name = name;
        _faculty = faculty;
        _streams = new List<Stream>(MaxNumberOfStreams);
    }

    public string Name => _name;
    public char Faculty => _faculty;
    public IReadOnlyList<Stream> Streams => _streams;

    public Stream AddStream(Stream stream)
    {
        if (stream is null)
            throw StreamException.NullArgument();
        if (_streams.Count >= MaxNumberOfStreams)
            throw CourseException.TooManyStreams(MaxNumberOfStreams);
        _streams.Add(stream);
        return stream;
    }
}