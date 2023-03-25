using Reports.DAL.Models;

namespace Reports.DAL.Interfaces;

public interface IAccountRepository
{
    List<Account> GetAll();
    Account GetById(Guid id);
    Account GetByEmail(string email);
    void Add(Account account);
    void Delete(Guid id);
}