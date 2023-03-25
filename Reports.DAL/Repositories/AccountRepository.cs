using Reports.DAL.Interfaces;
using Reports.DAL.Models;
using Reports.DAL.Utils;

namespace Reports.DAL.Repositories;

public class AccountRepository : IAccountRepository
{
    private List<Account> _accounts;

    public AccountRepository()
    {
        _accounts = new List<Account>();
    }

    public List<Account> Accounts => _accounts;

    public List<Account> GetAll()
    {
        return _accounts.ToList();
    }

    public Account GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        return _accounts.FirstOrDefault(a => a.Id == id) ?? throw new DatabaseException("Account is null.");
    }

    public Account GetByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DatabaseException("Email cannot be null.");
        return _accounts.FirstOrDefault(a => a.Email == email) ?? throw new DatabaseException("Account is null.");
    }

    public void Add(Account account)
    {
        if (account is null)
            throw new DatabaseException("Account cannot be null.");
        if (_accounts.Contains(account))
            throw new DatabaseException("Account already exists.");
        _accounts.Add(account);
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        Account account = GetById(id);
        if (!_accounts.Contains(account))
            throw new DatabaseException("Account doesn't exist.");
        _accounts.Remove(account);
    }
}