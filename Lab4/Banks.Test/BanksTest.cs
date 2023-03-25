using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Tools.Exceptions;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void DepositMoneyToAccountAndWithdrawIt()
    {
        const int Deposit = 100000;
        const int Withdraw = 500;
        CentralBank centralBank = CentralBank.GetInstance();
        Terms terms = new Terms(10, 10, 10, 10, 1000, 1, 500);
        Bank bank = new Bank("EldaBank", terms);
        Client client = new Client.Builder().FirstName("Eldar").LastName("Kasymov").Address("Pushkina Street, Kolotushkina House 1").Passport("2281337").Create();
        Account account = bank.CreateDebitAccount(client);
        Transaction depositTransaction = new Deposit(Deposit, account);
        Transaction withdrawTransaction = new Withdraw(Withdraw, account);

        centralBank.RegisterClient(client);
        centralBank.RegisterBank(bank);
        centralBank.ExecuteTransaction(depositTransaction);
        centralBank.ExecuteTransaction(withdrawTransaction);

        Assert.Equal(Deposit - Withdraw, account.Balance);
    }

    [Fact]
    public void WithdrawTooMuchMoneyFromCreditAccount()
    {
        const int Withdraw = 10001;
        CentralBank centralBank = CentralBank.GetInstance();
        Terms terms = new Terms(10, 10, 10, 10, 1000, 1, 500);
        Bank bank = new Bank("EldaBank", terms);
        Client client = new Client.Builder().FirstName("Eldar").LastName("Kasymov").Address("Pushkina Street, Kolotushkina House 1").Passport("2281337").Create();
        Account account = bank.CreateCreditAccount(client);
        Transaction transaction = new Withdraw(Withdraw, account);

        centralBank.RegisterClient(client);
        centralBank.RegisterBank(bank);

        Assert.Throws<BanksException>(() => centralBank.ExecuteTransaction(transaction));
    }

    [Fact]
    public void SubscriptionTest()
    {
        const decimal newInterest = 25;
        CentralBank centralBank = CentralBank.GetInstance();
        Terms terms = new Terms(10, 10, 10, 10, 1000, 1, 500);
        Bank bank = new Bank("EldaBank", terms);
        Client client = new Client.Builder().FirstName("Eldar").LastName("Kasymov").Address("Pushkina Street, Kolotushkina House 1").Passport("2281337").Create();
        Account account = new DepositAccount(client, bank);

        centralBank.RegisterClient(client);
        centralBank.RegisterBank(bank);
        bank.Terms.SetMaxDepositInterest(newInterest);
        bank.Attach(client);
        bank.SetNewTerms(bank.Terms);

        Assert.True(client.IsSubscribed);
        Assert.Contains(bank.Subscribers, observer => observer == client);
    }

    [Fact]
    public void TransferTest()
    {
        const decimal Deposit = 1001;
        const decimal Transfer = 1000;
        CentralBank centralBank = CentralBank.GetInstance();
        Terms terms = new Terms(10, 10, 10, 10, 1000, 1, 500);
        Bank bank = new Bank("EldaBank", terms);
        Client client1 = new Client.Builder().FirstName("Eldar").LastName("Kasymov").Address("Pushkina Street, Kolotushkina House 1").Passport("2281337").Create();
        Client client2 = new Client.Builder().FirstName("Polina").LastName("Khartanovich").Address("Pushkina Street, Kolotushkina House 2").Passport("1337228").Create();
        Account account1 = new DebitAccount(client1, bank);
        Account account2 = new DebitAccount(client2, bank);
        Transaction deposit = new Deposit(Deposit, account1);
        Transaction transfer = new Transfer(Transfer, account1, account2);

        centralBank.RegisterClient(client1);
        centralBank.RegisterClient(client2);
        centralBank.RegisterBank(bank);
        centralBank.ExecuteTransaction(deposit);
        centralBank.ExecuteTransaction(transfer);

        Assert.Equal(Deposit - Transfer, account1.Balance);
        Assert.Equal(Transfer, account2.Balance);
    }

    [Fact]
    public void CancelTransferTest()
    {
        const decimal StartCredits = 0;
        const decimal Deposit = 1001;
        const decimal Transfer = 1000;
        CentralBank centralBank = CentralBank.GetInstance();
        Terms terms = new Terms(10, 10, 10, 10, 1000, 1, 500);
        Bank bank = new Bank("EldaBank", terms);
        Client client1 = new Client.Builder().FirstName("Eldar").LastName("Kasymov").Address("Pushkina Street, Kolotushkina House 1").Passport("2281337").Create();
        Client client2 = new Client.Builder().FirstName("Polina").LastName("Khartanovich").Address("Pushkina Street, Kolotushkina House 2").Passport("1337228").Create();
        Account account1 = new DebitAccount(client1, bank);
        Account account2 = new DebitAccount(client2, bank);
        Transaction deposit = new Deposit(Deposit, account1);
        Transaction transfer = new Transfer(Transfer, account1, account2);

        centralBank.RegisterClient(client1);
        centralBank.RegisterClient(client2);
        centralBank.RegisterBank(bank);
        centralBank.ExecuteTransaction(deposit);
        centralBank.ExecuteTransaction(transfer);
        centralBank.CancelTransaction(transfer);

        Assert.Equal(Deposit, account1.Balance);
        Assert.Equal(StartCredits, account2.Balance);
    }
}