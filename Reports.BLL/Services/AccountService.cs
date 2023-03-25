using Reports.BLL.Interfaces;
using Reports.BLL.Utils;
using Reports.DAL.Interfaces;
using Reports.DAL.Models;

namespace Reports.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ApplicationLogicException("Unit of Work cannot be null.");
    }

    public Account GetById(Guid id)
    {
        return _unitOfWork.AccountRepository.GetById(id);
    }

    public Account GetByEmail(string email)
    {
        return _unitOfWork.AccountRepository.GetByEmail(email);
    }

    public List<Account> GetAll()
    {
        return _unitOfWork.AccountRepository.GetAll();
    }

    public void Add(Account account)
    {
        _unitOfWork.AccountRepository.Add(account);
    }

    public void Delete(Guid id)
    {
        _unitOfWork.AccountRepository.Delete(id);
    }

    public Account Create(string email, string password)
    {
        return new Account(email, password);
    }

    public bool ValidateCredentials(string email, string password)
    {
        return GetByEmail(email).Password == password;
    }
}