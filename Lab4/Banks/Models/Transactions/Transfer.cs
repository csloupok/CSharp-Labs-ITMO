using Banks.Models.Accounts;
using Banks.Tools.Exceptions;

namespace Banks.Models.Transactions;

public class Transfer : Transaction
{
    public Transfer(decimal credits, Account sender, Account recipient)
        : base(credits, sender, recipient)
    {
    }

    public override void Execute()
    {
        if (Sender is null)
            throw new BanksException("Sender is null.");
        if (Recipient is null)
            throw new BanksException("Recipient is null.");
        if (IsExecuted)
            throw new BanksException("Can't execute operation twice.");
        if (IsCanceled)
            throw new BanksException("Operation was canceled.");
        Sender.Withdraw(Credits);
        Recipient.Deposit(Credits);
        Executed();
    }

    public override void Cancel()
    {
        if (Sender is null)
            throw new BanksException("Sender is null.");
        if (Recipient is null)
            throw new BanksException("Recipient is null.");
        if (!IsExecuted)
            throw new BanksException("Operation was not executed.");
        if (IsCanceled)
            throw new BanksException("Can't cancel operation twice.");
        Sender.Deposit(Credits);
        Recipient.Withdraw(Credits);
        Canceled();
    }
}