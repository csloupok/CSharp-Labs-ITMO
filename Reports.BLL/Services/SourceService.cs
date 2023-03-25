using Reports.BLL.Interfaces;
using Reports.BLL.Utils;
using Reports.DAL.Interfaces;
using Reports.DAL.Models;

namespace Reports.BLL.Services;

public class SourceService : ISourceService
{
    private readonly IUnitOfWork _unitOfWork;

    public SourceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ApplicationLogicException("Unit of Work cannot be null.");
    }

    public List<Source> GetAll()
    {
        return _unitOfWork.SourceRepository.GetAll();
    }

    public Source GetById(Guid id)
    {
        return _unitOfWork.SourceRepository.GetById(id);
    }

    public void Add(Source source)
    {
        _unitOfWork.SourceRepository.Add(source);
    }

    public void Delete(Guid id)
    {
        _unitOfWork.SourceRepository.Delete(id);
    }

    public Source Create(string type, Account account)
    {
        return new Source(type, account);
    }
}
