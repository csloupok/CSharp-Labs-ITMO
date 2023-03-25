using Banks.Interfaces;
using Banks.Models.Accounts;
using Banks.Tools.Exceptions;

namespace Banks.Models.Transactions;

public abstract class Transaction : ICommand
{
    private decimal _credits;
    private Guid _id;
    private bool _isExecuted;
    private bool _isCanceled;
    private Account? _sender;
    private Account? _recipient;

    public Transaction(decimal credits, Account? sender = null, Account? recipient = null)
    {
        _credits = credits;
        _id = Guid.NewGuid();
        _isExecuted = false;
        _isCanceled = false;
        _sender = sender;
        _recipient = recipient;
    }

    public decimal Credits => _credits;
    public Guid Id => _id;
    public bool IsExecuted => _isExecuted;
    public bool IsCanceled => _isCanceled;
    public Account? Sender => _sender;
    public Account? Recipient => _recipient;
    public abstract void Execute();
    public abstract void Cancel();
    protected void Executed()
    {
        _isExecuted = true;
    }

    protected void Canceled()
    {
        _isCanceled = true;
    }
}