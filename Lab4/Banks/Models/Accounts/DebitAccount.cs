using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools.Exceptions;

namespace Banks.Models.Accounts;

public class DebitAccount : Account
{
    private decimal _interestCredits;

    public DebitAccount(Client client, Bank bank)
        : base(client, bank)
    {
        _interestCredits = MinAmountOfCredits;
    }

    public override void Deposit(decimal amount)
    {
        if (amount < MinAmountOfCredits)
            throw new BanksException("Number of credits can't be negative.");
        ChangeBalance(Balance + amount);
    }

    public override void Withdraw(decimal amount)
    {
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
        _interestCredits += Balance * Bank.Terms.CalculateDailyInterest(Bank.Terms.DebitInterest, CentralBank.GetInstance().CurrentDate);
        if (CentralBank.GetInstance().CurrentDate.Day != CreationDate.Day) return;
        ChangeBalance(Balance + _interestCredits);
        _interestCredits = 0;
    }
}