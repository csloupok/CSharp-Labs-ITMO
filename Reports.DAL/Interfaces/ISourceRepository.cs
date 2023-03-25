using Reports.DAL.Models;

namespace Reports.DAL.Interfaces;

public interface ISourceRepository
{
    List<Source> GetAll();
    Source GetById(Guid id);
    void Add(Source source);
    void Delete(Guid id);
}
