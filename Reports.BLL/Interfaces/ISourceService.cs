using Reports.DAL.Models;

namespace Reports.BLL.Interfaces;

public interface ISourceService
{
    Source GetById(Guid id);
    List<Source> GetAll();
    void Add(Source source);
    void Delete(Guid id);
    Source Create(string type, Account account);
}
