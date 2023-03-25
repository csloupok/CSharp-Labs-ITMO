using System.Runtime.InteropServices.ComTypes;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools.Exceptions;

namespace Banks.Models.Accounts;

public abstract class Account
{
    protected const int MinAmountOfCredits = 0;
    private Client _client;
    private Bank _bank;
    private decimal _balance;
    private Guid _id;
    private DateTime _creationDate;
    protected Account(Client client, Bank bank)
    {
        _client = client ?? throw new BanksException("Client is null.");
        _bank = bank ?? throw new BanksException("Bank is null.");
        _balance = MinAmountOfCredits;
        _id = Guid.NewGuid();
        _creationDate = DateTime.Today;
    }

    public Client Client => _client;
    public Bank Bank => _bank;
    public decimal Balance => _balance;
    public Guid Id => _id;
    public DateTime CreationDate => _creationDate;

    public abstract void Deposit(decimal amount);
    public abstract void Withdraw(decimal amount);
    public abstract void RefreshAccount();
    protected void ChangeBalance(decimal balance)
    {
        _balance = balance;
    }
}