using Banks.Models.Accounts;
using Banks.Tools.Exceptions;

namespace Banks.Models.Transactions;

public class Deposit : Transaction
{
    public Deposit(decimal credits, Account recipient)
        : base(credits, null, recipient)
    {
    }

    public override void Execute()
    {
        if (Recipient is null)
            throw new BanksException("Recipient is null.");
        if (IsExecuted)
            throw new BanksException("Can't execute operation twice.");
        if (IsCanceled)
            throw new BanksException("Operation was canceled.");
        Recipient.Deposit(Credits);
        Executed();
    }

    public override void Cancel()
    {
        if (Recipient is null)
            throw new BanksException("Recipient is null.");
        if (!IsExecuted)
            throw new BanksException("Operation was not executed.");
        if (IsCanceled)
            throw new BanksException("Can't cancel operation twice.");
        Recipient.Withdraw(Credits);
        Canceled();
    }
}