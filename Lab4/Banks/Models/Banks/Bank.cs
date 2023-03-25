using Banks.Interfaces;
using Banks.Models.Accounts;
using Banks.Models.Clients;
using Banks.Tools.Exceptions;

namespace Banks.Models.Banks;

public class Bank : ISubject
{
    private string _name;
    private Terms _terms;
    private Guid _id;
    private List<Account> _accounts;
    private List<IObserver> _observers;

    public Bank(string name, Terms terms)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Bank name is null.");
        _name = name;
        _terms = terms ?? throw new BanksException("Terms are null.");
        _id = Guid.NewGuid();
        _accounts = new List<Account>();
        _observers = new List<IObserver>();
    }

    public string Name => _name;
    public Terms Terms => _terms;
    public IReadOnlyList<Account> Accounts => _accounts;
    public IReadOnlyList<IObserver> Subscribers => _observers;

    public DebitAccount CreateDebitAccount(Client client)
    {
        if (client is null)
            throw new BanksException("Client is null.");
        DebitAccount debitAccount = new DebitAccount(client, this);
        _accounts.Add(debitAccount);
        return debitAccount;
    }

    public DepositAccount CreateDepositAccount(Client client)
    {
        if (client is null)
            throw new BanksException("Client is null.");
        DepositAccount depositAccount = new DepositAccount(client, this);
        _accounts.Add(depositAccount);
        return depositAccount;
    }

    public CreditAccount CreateCreditAccount(Client client)
    {
        if (client is null)
            throw new BanksException("Client is null.");
        CreditAccount creditAccount = new CreditAccount(client, this);
        _accounts.Add(creditAccount);
        return creditAccount;
    }

    public void SetNewTerms(Terms terms)
    {
        _terms = terms ?? throw new BanksException("Terms are null.");
        Notify();
    }

    public void RefreshAccounts()
    {
        foreach (Account account in _accounts)
            account.RefreshAccount();
    }

    public void Attach(IObserver observer)
    {
        if (observer is null)
            throw new BanksException("Subscriber is null.");
        observer.Subscribe();
        _observers.Add(observer);
        Console.WriteLine("Bank: Client subscribed.");
    }

    public void Detach(IObserver observer)
    {
        if (observer is null)
            throw new BanksException("Subscriber is null.");
        observer.Unsubscribe();
        _observers.Remove(observer);
        Console.WriteLine("Bank: Client unsubscribed.");
    }

    public void Notify()
    {
        Console.WriteLine("Bank: Notifying clients...");
        foreach (IObserver observer in _observers)
            observer.Update(this);
    }
}