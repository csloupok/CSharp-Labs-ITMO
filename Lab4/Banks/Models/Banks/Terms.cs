using Banks.Tools.Exceptions;

namespace Banks.Models.Banks;

public class Terms
{
    public const decimal MinInterest = 0;
    public const decimal MaxInterest = 25;
    public const decimal MaxFee = 50;
    private const int DaysInRegYear = 365;
    private const int DaysInLeapYear = 366;
    private const decimal MinFee = 0;
    private const decimal MinLimit = 0;
    private decimal _debitInterest;
    private decimal _minDepositCredits = 50000;
    private decimal _maxDepositCredits = 10000;
    private decimal _minDepositInterest;
    private decimal _midDepositInterest;
    private decimal _maxDepositInterest;
    private decimal _creditLimit;
    private decimal _creditFee;
    private decimal _unconfirmedLimit;

    public Terms(
        decimal debitInterest,
        decimal minDepositInterest,
        decimal midDepositInterest,
        decimal maxDepositInterest,
        decimal creditLimit,
        decimal creditFee,
        decimal unconfirmedLimit)
    {
        if (debitInterest is < MinInterest or > MaxInterest)
            throw new BanksException($"Debit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (minDepositInterest is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (minDepositInterest > midDepositInterest || minDepositInterest > maxDepositInterest)
            throw new BanksException("Minimal deposit interest can't be higher than other.");
        if (midDepositInterest > maxDepositInterest)
            throw new BanksException("Middle deposit interest can't be higher than maximal.");
        if (midDepositInterest is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (maxDepositInterest is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (creditLimit < MinLimit)
            throw new BanksException($"Credit limit can't be less than {MinLimit}.");
        if (unconfirmedLimit < MinLimit)
            throw new BanksException($"Unconfirmed limit can't be less than {MinLimit}.");
        if (creditFee is < MinFee or > MaxFee)
            throw new BanksException($"Credit fee must be within [{MinFee} - {MaxFee}] percent range.");
        _debitInterest = debitInterest;
        _minDepositInterest = minDepositInterest;
        _midDepositInterest = midDepositInterest;
        _maxDepositInterest = maxDepositInterest;
        _creditLimit = creditLimit;
        _creditFee = creditFee;
        _unconfirmedLimit = unconfirmedLimit;
    }

    public decimal DebitInterest => _debitInterest;
    public decimal MinDepositInterest => _minDepositInterest;
    public decimal MidDepositInterest => _midDepositInterest;
    public decimal MaxDepositCredits => _maxDepositCredits;
    public decimal MinDepositCredits => _minDepositCredits;
    public decimal MaxDepositInterest => _maxDepositInterest;
    public decimal CreditLimit => _creditLimit;
    public decimal CreditFee => _creditFee;
    public decimal UnconfirmedLimit => _unconfirmedLimit;

    public void SetDebitInterest(decimal value)
    {
        if (value is < MinInterest or > MaxInterest)
            throw new BanksException($"Debit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        _debitInterest = value;
    }

    public void SetMinDepositInterest(decimal value)
    {
        if (value is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (value > MidDepositInterest || value > MaxDepositInterest)
            throw new BanksException("Minimal deposit interest can't be higher than other.");
        _minDepositInterest = value;
    }

    public void SetMidDepositInterest(decimal value)
    {
        if (value is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (value < MinDepositInterest || value > MaxDepositInterest)
            throw new BanksException("Middle deposit interest can't be higher than maximal or lower than minimal.");
        _midDepositInterest = value;
    }

    public void SetMaxDepositInterest(decimal value)
    {
        if (value is < MinInterest or > MaxInterest)
            throw new BanksException($"Deposit interest must be within [{MinInterest} - {MaxInterest}] percent range.");
        if (value < MidDepositInterest || value < MinDepositInterest)
            throw new BanksException("Maximal deposit interest can't be lower than other.");
        _maxDepositInterest = value;
    }

    public void SetMinDepositCredits(decimal value)
    {
        if (value < MinLimit)
            throw new BanksException("Amount of credits can't be negative.");
        _minDepositCredits = value;
    }

    public void SetMaxDepositCredits(decimal value)
    {
        if (value < MinLimit)
            throw new BanksException("Amount of credits can't be negative.");
        _maxDepositCredits = value;
    }

    public void SetCreditLimit(decimal value)
    {
        if (value < MinLimit)
            throw new BanksException($"Credit limit can't be less than {MinLimit}.");
        _creditLimit = value;
    }

    public void SetCreditFee(decimal value)
    {
        if (value is < MinFee or > MaxFee)
            throw new BanksException($"Credit fee must be within [{MinFee} - {MaxFee}] percent range.");
        _creditFee = value;
    }

    public void SetUnconfirmedLimit(decimal value)
    {
        if (value < MinLimit)
            throw new BanksException($"Unconfirmed limit can't be less than {MinLimit}.");
        _unconfirmedLimit = value;
    }

    public decimal CalculateDailyInterest(decimal interest, DateTime date)
    {
        if (DateTime.IsLeapYear(date.Year))
            return interest / 100 / DaysInLeapYear;
        return interest / 100 / DaysInRegYear;
    }
}