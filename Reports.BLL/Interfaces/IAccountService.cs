
using Reports.DAL.Models;

namespace Reports.BLL.Interfaces;

public interface IAccountService
{
    Account GetById(Guid id);
    Account GetByEmail(string email);
    List<Account> GetAll();
    void Add(Account account);
    void Delete(Guid id);
    Account Create(string email, string password);
    bool ValidateCredentials(string email, string password);
}