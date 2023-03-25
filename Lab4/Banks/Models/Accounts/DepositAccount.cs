using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools.Exceptions;

namespace Banks.Models.Accounts;

public class DepositAccount : Account
{
    private const int SpanYearsOfAccount = 1;
    private DateTime _closingDate;
    private decimal _interestCredits;
    private decimal _currentInterest;

    public DepositAccount(Client client, Bank bank)
        : base(client, bank)
    {
        _closingDate = CreationDate.AddYears(SpanYearsOfAccount);
        _interestCredits = MinAmountOfCredits;
        _currentInterest = bank.Terms.MinDepositInterest;
    }

    public decimal CurrentInterest => _currentInterest;

    public override void Deposit(decimal amount)
    {
        if (amount < MinAmountOfCredits)
            throw new BanksException("Number of credits can't be negative.");
        ChangeBalance(Balance + amount);
    }

    public override void Withdraw(decimal amount)
    {
        if (CentralBank.GetInstance().CurrentDate < _closingDate)
            throw new BanksException("Account is not closed yet.");
        if (amount < MinAmountOfCredits)
            throw new BanksException("Number of credits can't be negative.");
        if (Balance < amount)
            throw new BanksException("Not enough credits.");
        if (!Client.IsConfirmed() && amount > Bank.Terms.UnconfirmedLimit)
            throw new BanksException("Unconfirmed limit is exceeded.");
        ChangeBalance(Balance - amount);
    }

    public override void RefreshAccount()
    {
        if (Balance < Bank.Terms.MinDepositCredits)
            _currentInterest = Bank.Terms.MinDepositInterest;
        if (Balance > Bank.Terms.MinDepositCredits && Balance < Bank.Terms.MaxDepositCredits)
            _currentInterest = Bank.Terms.MidDepositInterest;
        if (Balance > Bank.Terms.MaxDepositCredits)
            _currentInterest = Bank.Terms.MaxDepositInterest;
        _interestCredits += Balance * Bank.Terms.CalculateDailyInterest(CurrentInterest, CentralBank.GetInstance().CurrentDate);
        if (CentralBank.GetInstance().CurrentDate.Day != CreationDate.Day) return;
        ChangeBalance(Balance + _interestCredits);
        _interestCredits = 0;
    }
}