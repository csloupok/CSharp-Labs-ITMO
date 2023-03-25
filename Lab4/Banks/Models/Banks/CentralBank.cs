using Banks.Models.Accounts;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Tools.Exceptions;

namespace Banks.Models.Banks;

public class CentralBank
{
    private const int MinCount = 0;
    private const int DefaultCount = 1;
    private const int CountOfMonthsInYear = 12;
    private static readonly object Lock = new object();
    private static CentralBank? _centralBank;
    private List<Bank> _banks;
    private List<Client> _clients;
    private List<Transaction> _transactions;
    private DateTime _currentDate;

    private CentralBank()
    {
        _banks = new List<Bank>();
        _clients = new List<Client>();
        _transactions = new List<Transaction>();
        _currentDate = DateTime.Today;
    }

    public DateTime CurrentDate => _currentDate;

    public static CentralBank GetInstance()
    {
        if (_centralBank != null) return _centralBank;
        lock (Lock)
            _centralBank ??= new CentralBank();
        return _centralBank;
    }

    public void ExecuteTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Transaction is null.");
        transaction.Execute();
        _transactions.Add(transaction);
    }

    public void CancelTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Transaction is null.");
        transaction.Cancel();
    }

    public void RegisterBank(Bank bank)
    {
        if (bank is null)
            throw new BanksException("Bank is null.");
        _banks.Add(bank);
    }

    public void RegisterClient(Client client)
    {
        if (client is null)
            throw new BanksException("Client is null.");
        _clients.Add(client);
    }

    public void RewindDay(int count = DefaultCount)
    {
        if (count <= MinCount)
            throw new BanksException("Count of days can't be negative.");
        for (int i = MinCount; i < count; i++)
        {
            _currentDate += TimeSpan.FromDays(DefaultCount);
            foreach (Bank bank in _banks)
                bank.RefreshAccounts();
        }
    }

    public void RewindMonth(int count = DefaultCount)
    {
        if (count <= MinCount)
            throw new BanksException("Count of months can't be negative.");
        for (int i = MinCount; i < count; i++)
            RewindDay(DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month));
    }

    public void RewindYear(int count = DefaultCount)
    {
        if (count <= MinCount)
            throw new BanksException("Count of years can't be negative.");
        for (int i = MinCount; i < count; i++)
            RewindMonth(CountOfMonthsInYear);
    }

    public Bank? FindBank(string name)
    {
        return _banks.Find(bank => bank.Name == name);
    }

    public Client? FindClient(string id)
    {
        return _clients.Find(client => client.Id.ToString() == id);
    }

    public Account? FindAccount(Bank bank, string id)
    {
        if (bank is null)
            throw new BanksException("Bank is null.");
        return bank.Accounts.FirstOrDefault(account => account.Id.ToString() == id);
    }
}