using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools.Exceptions;

namespace Banks.Models.Accounts;

public class CreditAccount : Account
{
    private decimal _feeCredits;
    public CreditAccount(Client client, Bank bank)
        : base(client, bank) { }

    public override void Withdraw(decimal amount)
    {
        if (amount < MinAmountOfCredits)
            throw new BanksException("Number of credits can't be negative.");
        if (!Client.IsConfirmed() && amount > Bank.Terms.UnconfirmedLimit)
            throw new BanksException("Unconfirmed limit is exceeded.");
        if (Balance - amount < -Bank.Terms.CreditLimit)
            throw new BanksException("Credit limit is exceeded.");
        ChangeBalance(Balance - amount);
    }

    public override void Deposit(decimal amount)
    {
        if (amount < MinAmountOfCredits)
            throw new BanksException("Number of credits can't be negative.");
        ChangeBalance(Balance + amount);
    }

    public override void RefreshAccount()
    {
       if (Balance < MinAmountOfCredits)
           _feeCredits -= Balance * Bank.Terms.CalculateDailyInterest(Bank.Terms.CreditFee, CentralBank.GetInstance().CurrentDate);
       if (CentralBank.GetInstance().CurrentDate.Day != CreationDate.Day) return;
       ChangeBalance(Balance + _feeCredits);
       _feeCredits = 0;
    }
}