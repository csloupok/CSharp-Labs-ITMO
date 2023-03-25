using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Spectre.Console;

namespace Banks.Console;

public static class Console
{
    private const int MinAmount = 0;
    private static CentralBank _centralBank = CentralBank.GetInstance();

    public static void Main(string[] args)
    {
        AnsiConsole.Write(
            new FigletText("Banks App")
                .LeftJustified()
                .Color(Color.Green));
        while (true)
        {
            string chosenOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select [underline blue]console[/] to use:")
                    .AddChoices("Central Bank", "Bank", "Client"));
            string bankName;
            Bank? bank;
            string clientId;
            Client? client;
            switch (chosenOption)
            {
                case "Central Bank":
                    chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select [underline blue]command[/]:")
                        .AddChoices("Register Client", "Register Bank", "Rewind Time"));
                    switch (chosenOption)
                    {
                        case "Register Client":
                            string firstName = AnsiConsole.Ask<string>("What's your [green]first name[/]?");
                            string lastName = AnsiConsole.Ask<string>("What's your [green]last name[/]?");
                            if (AnsiConsole.Confirm("Do you want to add passport and address?"))
                            {
                                string passport = AnsiConsole.Ask<string>("Enter your passport ID:");
                                string address = AnsiConsole.Ask<string>("Enter your address:");
                                client = new Client.Builder().FirstName(firstName).LastName(lastName).Passport(passport).Address(address).Create();
                            }

                            client = new Client.Builder().FirstName(firstName).LastName(lastName).Create();
                            CentralBank.GetInstance().RegisterClient(client);
                            AnsiConsole.Markup($"Don't forget: your ID is [underline red]{client.Id}[/]");
                            break;
                        case "Register Bank":
                            bankName = AnsiConsole.Ask<string>("What's your [green]bank name[/]?");
                            decimal debitInterest = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        $"What's [green]debit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                    .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                    .Validate(rate =>
                                    {
                                        return rate switch
                                        {
                                            < Terms.MinInterest or > Terms.MaxInterest => ValidationResult.Error(
                                                $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));
                            decimal minDepositInterest = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        $"What's [green]minimal deposit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                    .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                    .Validate(rate =>
                                    {
                                        return rate switch
                                        {
                                            < Terms.MinInterest or > Terms.MaxInterest => ValidationResult.Error(
                                                $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));
                            decimal midDepositInterest = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        $"What's [green]middle deposit interest[/]?\nIt must be within {minDepositInterest} - {Terms.MaxInterest} rate range.\n")
                                    .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                    .Validate(rate =>
                                    {
                                        return rate switch
                                        {
                                            < Terms.MinInterest or > Terms.MaxInterest => ValidationResult.Error(
                                                $"[red]It must be within {minDepositInterest} - {Terms.MaxInterest} rate range![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));
                            decimal maxDepositInterest = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        $"What's [green]maximal deposit interest[/]?\nIt must be within {midDepositInterest} - {Terms.MaxInterest} rate range.\n")
                                    .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                    .Validate(rate =>
                                    {
                                        return rate switch
                                        {
                                            < Terms.MinInterest or > Terms.MaxInterest => ValidationResult.Error(
                                                $"[red]It must be within {midDepositInterest} - {Terms.MaxInterest} rate range![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));
                            decimal creditLimit = AnsiConsole.Ask<decimal>("What's [green]credit limit[/]?");
                            decimal creditFee = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        $"What's [green]credit fee[/]?\nIt must be under {Terms.MaxFee}.\n")
                                    .ValidationErrorMessage("[red]That's not a valid fee.[/]")
                                    .Validate(rate =>
                                    {
                                        return rate switch
                                        {
                                            > Terms.MaxFee => ValidationResult.Error(
                                                $"[red]It must be under {Terms.MaxFee}![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));
                            decimal unconfirmedLimit = AnsiConsole.Ask<decimal>("What's [green]unconfirmed limit[/]?");
                            Terms terms = new Terms(
                                debitInterest,
                                minDepositInterest,
                                midDepositInterest,
                                maxDepositInterest,
                                creditLimit,
                                creditFee,
                                unconfirmedLimit);
                            bank = new Bank(bankName, terms);
                            CentralBank.GetInstance().RegisterBank(bank);
                            break;
                        case "Rewind Time":
                            chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [underline blue]command[/]:")
                                .AddChoices("Rewind Days", "Rewind Months", "Rewind Years"));
                            switch (chosenOption)
                            {
                                case "Rewind Days":
                                    int countOfDays = AnsiConsole.Prompt(
                                        new TextPrompt<int>(
                                                "How many days to skip?\n")
                                            .ValidationErrorMessage("[red]You can't go back in time :([/]")
                                            .Validate(days =>
                                            {
                                                return days switch
                                                {
                                                    <= MinAmount => ValidationResult.Error(
                                                        "[red]You can't go back in time :([/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    CentralBank.GetInstance().RewindDay(countOfDays);
                                    break;
                                case "Rewind Months":
                                    int countOfMonths = AnsiConsole.Prompt(
                                        new TextPrompt<int>(
                                                "How many months to skip?\n")
                                            .ValidationErrorMessage("[red]You can't go back in time :([/]")
                                            .Validate(months =>
                                            {
                                                return months switch
                                                {
                                                    <= MinAmount => ValidationResult.Error(
                                                        "[red]You can't go back in time :([/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    CentralBank.GetInstance().RewindMonth(countOfMonths);
                                    break;
                                case "Rewind Years":
                                    int countOfYears = AnsiConsole.Prompt(
                                        new TextPrompt<int>(
                                                "How many years to skip?\n")
                                            .ValidationErrorMessage("[red]You can't go back in time :([/]")
                                            .Validate(years =>
                                            {
                                                return years switch
                                                {
                                                    <= MinAmount => ValidationResult.Error(
                                                        "[red]You can't go back in time :([/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    CentralBank.GetInstance().RewindYear(countOfYears);
                                    break;
                            }

                            break;
                    }

                    break;
                case "Bank":
                    bankName = AnsiConsole.Ask<string>("What's [green]bank name[/]?");
                    bank = _centralBank.FindBank(bankName);
                    if (bank is null)
                    {
                        AnsiConsole.Markup("[underline red]Bank with this name doesn't exist![/]");
                        break;
                    }

                    chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select [underline blue]command[/]:")
                        .AddChoices("Register account", "Update terms"));
                    switch (chosenOption)
                    {
                        case "Register account":
                            clientId = AnsiConsole.Ask<string>("What's [green]client's ID[/]?");
                            client = _centralBank.FindClient(clientId);
                            if (client is null)
                            {
                                AnsiConsole.Markup("[underline red]Client with this ID doesn't exist![/]");
                                break;
                            }

                            string type = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [underline blue]account type[/]:")
                                .AddChoices("Debit", "Deposit", "Credit"));
                            switch (type)
                            {
                                case "Debit":
                                    bank.CreateDebitAccount(client);
                                    break;
                                case "Deposit":
                                    bank.CreateDepositAccount(client);
                                    break;
                                case "Credit":
                                    bank.CreateCreditAccount(client);
                                    break;
                            }

                            break;
                        case "Update terms":
                            chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [underline blue]command[/]:")
                                .AddChoices(
                                    "Debit interest",
                                    "Minimal Deposit interest",
                                    "Middle Deposit interest",
                                    "Maximal Deposit interest",
                                    "Credit Fee",
                                    "Credit Limit",
                                    "Unconfirmed Limit"));
                            switch (chosenOption)
                            {
                                case "Debit interest":
                                    decimal value = AnsiConsole.Prompt(
                                        new TextPrompt<decimal>(
                                                $"What's [green]debit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                            .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                            .Validate(rate =>
                                            {
                                                return rate switch
                                                {
                                                    < Terms.MinInterest or > Terms.MaxInterest =>
                                                        ValidationResult.Error(
                                                            $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    bank.Terms.SetDebitInterest(value);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Deposit Minimal interest":
                                    value = AnsiConsole.Prompt(
                                        new TextPrompt<decimal>(
                                                $"What's [green]minimal deposit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                            .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                            .Validate(rate =>
                                            {
                                                return rate switch
                                                {
                                                    < Terms.MinInterest or > Terms.MaxInterest =>
                                                        ValidationResult.Error(
                                                            $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    bank.Terms.SetMinDepositInterest(value);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Middle Deposit interest":
                                    value = AnsiConsole.Prompt(
                                        new TextPrompt<decimal>(
                                                $"What's [green]middle deposit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                            .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                            .Validate(rate =>
                                            {
                                                return rate switch
                                                {
                                                    < Terms.MinInterest or > Terms.MaxInterest =>
                                                        ValidationResult.Error(
                                                            $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    bank.Terms.SetMidDepositInterest(value);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Maximal Deposit interest":
                                    value = AnsiConsole.Prompt(
                                        new TextPrompt<decimal>(
                                                $"What's [green]maximal deposit interest[/]?\nIt must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range.\n")
                                            .ValidationErrorMessage("[red]That's not a valid rate.[/]")
                                            .Validate(rate =>
                                            {
                                                return rate switch
                                                {
                                                    < Terms.MinInterest or > Terms.MaxInterest =>
                                                        ValidationResult.Error(
                                                            $"[red]It must be within {Terms.MinInterest} - {Terms.MaxInterest} rate range![/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    bank.Terms.SetMaxDepositInterest(value);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Credit Fee":
                                    value = AnsiConsole.Prompt(
                                        new TextPrompt<decimal>(
                                                $"What's [green]credit fee[/]?\nIt must be under {Terms.MaxFee}.\n")
                                            .ValidationErrorMessage("[red]That's not a valid fee.[/]")
                                            .Validate(fee =>
                                            {
                                                return fee switch
                                                {
                                                    > Terms.MaxFee => ValidationResult.Error(
                                                        $"[red]It must be under {Terms.MaxFee}![/]"),
                                                    _ => ValidationResult.Success(),
                                                };
                                            }));
                                    bank.Terms.SetCreditFee(value);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Credit Limit":
                                    int limit = AnsiConsole.Ask<int>("What's [green]credit limit[/]?");
                                    bank.Terms.SetCreditLimit(limit);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                                case "Unconfirmed Limit":
                                    limit = AnsiConsole.Ask<int>("What's [green]unconfirmed limit[/]?");
                                    bank.Terms.SetUnconfirmedLimit(limit);
                                    bank.SetNewTerms(bank.Terms);
                                    break;
                            }

                            break;
                    }

                    break;
                case "Client":
                    clientId = AnsiConsole.Ask<string>("What's [green]client's ID[/]?");
                    client = _centralBank.FindClient(clientId);
                    if (client is null)
                    {
                        AnsiConsole.Markup("[underline red]Client with this ID doesn't exist![/]");
                        break;
                    }

                    chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select [underline blue]command[/]:")
                        .AddChoices("Credits operations", "Manage subscription"));
                    switch (chosenOption)
                    {
                        case "Credits operations":
                            bankName = AnsiConsole.Ask<string>("What's [green]bank name[/]?");
                            bank = _centralBank.FindBank(bankName);
                            if (bank is null)
                            {
                                AnsiConsole.Markup("[underline red]Bank with this name doesn't exist![/]");
                                break;
                            }

                            string accountId = AnsiConsole.Ask<string>("What's [green]account ID[/]?");
                            Account? account = _centralBank.FindAccount(bank, accountId);
                            if (account is null)
                            {
                                AnsiConsole.Markup("[underline red]Account with this ID doesn't exist![/]");
                                break;
                            }

                            decimal value = AnsiConsole.Prompt(
                                new TextPrompt<decimal>(
                                        "How many [green]credits[/]?")
                                    .ValidationErrorMessage("[red]That's not a valid amount.[/]")
                                    .Validate(credits =>
                                    {
                                        return credits switch
                                        {
                                            < MinAmount =>
                                                ValidationResult.Error(
                                                    "[red]You can't enter negative amount![/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));

                            chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [underline blue]command[/]:")
                                .AddChoices("Deposit credits", "Transfer credits", "Withdraw credits"));
                            switch (chosenOption)
                            {
                                case "Deposit credits":
                                    Transaction transaction = new Deposit(value, account);
                                    _centralBank.ExecuteTransaction(transaction);
                                    break;
                                case "Transfer credits":
                                    AnsiConsole.Ask<string>("From what [green]account ID[/]?");
                                    Account? senderAccount = _centralBank.FindAccount(bank, accountId);
                                    if (senderAccount is null)
                                    {
                                        AnsiConsole.Markup("[underline red]Account with this ID doesn't exist![/]");
                                        break;
                                    }

                                    transaction = new Transfer(value, senderAccount, account);
                                    _centralBank.ExecuteTransaction(transaction);
                                    break;
                                case "Withdraw credits":
                                    transaction = new Withdraw(value, account);
                                    _centralBank.ExecuteTransaction(transaction);
                                    break;
                            }

                            break;
                        case "Manage subscription":
                            bankName = AnsiConsole.Ask<string>("What's [green]bank name[/]?");
                            bank = _centralBank.FindBank(bankName);
                            if (bank is null)
                            {
                                AnsiConsole.Markup("[underline red]Bank with this name doesn't exist![/]");
                                break;
                            }

                            chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [underline blue]command[/]:")
                                .AddChoices("Subscribe", "Unsubscribe"));
                            switch (chosenOption)
                            {
                                case "Subscribe":
                                    bank.Attach(client);
                                    break;
                                case "Unsubscribe":
                                    bank.Detach(client);
                                    break;
                            }

                            break;
                    }

                    break;
            }
        }
    }
}