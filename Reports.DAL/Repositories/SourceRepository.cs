using Reports.DAL.Interfaces;
using Reports.DAL.Models;
using Reports.DAL.Utils;

namespace Reports.DAL.Repositories;

public class SourceRepository : ISourceRepository
{
    private List<Source> _sources;

    public SourceRepository()
    {
        _sources = new List<Source>();
    }

    public List<Source> Sources => _sources;

    public List<Source> GetAll()
    {
        return _sources.ToList();
    }

    public Source GetById(Guid id)
    {
        return _sources.FirstOrDefault(e => e.Id == id) ?? throw new DatabaseException("Source is null.");
    }

    public void Add(Source source)
    {
        if (source is null)
            throw new DatabaseException("Source cannot be null.");
        if (_sources.Contains(source))
            throw new DatabaseException("Source already exists.");
        _sources.Add(source);
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        Source source = GetById(id);
        if (!_sources.Contains(source))
            throw new DatabaseException("Source doesn't exist.");
        _sources.Remove(source);
    }
}