using Reports.BLL.Services;
using Reports.DAL;
using Reports.DAL.Models;
using Reports.DAL.Repositories;
using Spectre.Console;

namespace Reports.PresentaionLayer;

public static class Console
{
    public static void Main(string[] args)
    {
        var accountRepository = new AccountRepository();
        var employeeRepository = new EmployeeRepository();
        var messageRepository = new MessageRepository();
        var sourceRepository = new SourceRepository();
        var reportRepository = new ReportRepository();

        var unitOfWork = new UnitOfWork(
            employeeRepository,
            messageRepository,
            sourceRepository,
            reportRepository,
            accountRepository);

        var accountService = new AccountService(unitOfWork);
        var employeeService = new EmployeeService(unitOfWork);
        var messageService = new MessageService(unitOfWork);
        var sourceService = new SourceService(unitOfWork);
        var reportService = new ReportService(unitOfWork);

        AnsiConsole.Write(
            new FigletText("Reports App")
                .LeftJustified()
                .Color(Color.Green));
        while (true)
        {
            string chosenOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select [underline blue]console[/] to use:")
                    .AddChoices("Director", "Employee"));
            string password;
            Employee employee;
            switch (chosenOption)
            {
                case "Director":
                    chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select [underline blue]command[/]:")
                        .AddChoices(
                            "Create account",
                            "Register employee",
                            "Add subordinate",
                            "Remove subordinate",
                            "Change supervisor",
                            "Create message",
                            "Create source"));
                    string email;
                    Account account;
                    string employeeId;
                    string subordinateId;
                    switch (chosenOption)
                    {
                        case "Create account":
                            email = AnsiConsole.Ask<string>("What [green]email[/]?");
                            password = AnsiConsole.Ask<string>("What [green]password[/]?");
                            account = accountService.Create(email, password);
                            accountService.Add(account);
                            break;
                        case "Register employee":
                            string name = AnsiConsole.Ask<string>("What [green]name[/]?");
                            email = AnsiConsole.Ask<string>("What [green]account (email)[/]?");
                            account = accountService.GetByEmail(email);
                            employee = employeeService.Create(name, account);
                            employeeService.Add(employee);
                            AnsiConsole.Markup($"Employee ID is [underline red]{employee.Id}[/]");
                            Thread.Sleep(5000);
                            break;
                        case "Add subordinate":
                            employeeId = AnsiConsole.Ask<string>("What [green]employee ID[/]?");
                            subordinateId = AnsiConsole.Ask<string>("What [green]subordinate ID[/]?");
                            employeeService.AddSubordinate(Guid.Parse(employeeId), Guid.Parse(subordinateId));
                            break;
                        case "Remove subordinate":
                            employeeId = AnsiConsole.Ask<string>("What [green]employee ID[/]?");
                            subordinateId = AnsiConsole.Ask<string>("What [green]subordinate ID[/]?");
                            employeeService.RemoveSubordinate(Guid.Parse(employeeId), Guid.Parse(subordinateId));
                            break;
                        case "Change supervisor":
                            employeeId = AnsiConsole.Ask<string>("What [green]employee ID[/]?");
                            string supervisorId = AnsiConsole.Ask<string>("What [green]supervisor ID[/]?");
                            employeeService.ChangeSupervisor(Guid.Parse(employeeId), Guid.Parse(supervisorId));
                            break;
                        case "Create message":
                            string sourceId = AnsiConsole.Ask<string>("What [green]source ID[/]?");
                            string text = AnsiConsole.Ask<string>("What [green]text[/]?");
                            employeeId = AnsiConsole.Ask<string>("What [green]employee ID[/]?");
                            Message message = messageService.Create(text, Guid.Parse(sourceId), Guid.Parse(employeeId));
                            messageService.Add(message);
                            break;
                        case "Create source":
                            string type = AnsiConsole.Ask<string>("What [green]type[/]?");
                            email = AnsiConsole.Ask<string>("What [green]account (email)[/]?");
                            account = accountService.GetByEmail(email);
                            Source source = sourceService.Create(type, account);
                            sourceService.Add(source);
                            AnsiConsole.Markup($"Source ID is [underline red]{source.Id}[/]");
                            Thread.Sleep(5000);
                            break;
                    }

                    break;
                case "Employee":
                    email = AnsiConsole.Ask<string>("What's [green]your email[/]?");
                    password = AnsiConsole.Ask<string>("What's [green]your password[/]?");
                    if (accountService.ValidateCredentials(email, password) is false)
                    {
                        AnsiConsole.Markup("[underline red]Wrong password![/]");
                        Thread.Sleep(5000);
                        break;
                    }

                    employee = employeeService.GetByEmail(email);
                    chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select [underline blue]command[/]:")
                        .AddChoices(
                            "View Messages",
                            "Process Messages",
                            "Generate Report"));
                    IReadOnlyList<Message>? messages;
                    switch (chosenOption)
                    {
                        case "View Messages":
                            messages = messageRepository.GetByEmployeeId(employee.Id);
                            foreach (Message m in messages)
                            {
                                AnsiConsole.Markup(m.Text);
                                messageService.ReceiveMessage(m.Id);
                                Thread.Sleep(5000);
                            }

                            break;
                        case "Process Messages":
                            messages = messageRepository.GetByEmployeeId(employee.Id);
                            foreach (Message m in messages)
                            {
                                messageService.ProcessMessage(m.Id);
                            }

                            AnsiConsole.Markup("Processed all messages.");
                            Thread.Sleep(5000);
                            break;
                        case "Generate Report":
                            string startDate = AnsiConsole.Ask<string>("What [green]start date[/]?");
                            string endDate = AnsiConsole.Ask<string>("What [green]end date[/]?");
                            var report =
                                reportService.GenerateReport(DateTime.Parse(startDate), DateTime.Parse(endDate));
                            AnsiConsole.Markup($"Amount of Processed messages : {report.ProcessedMessages.ToString()}\n");
                            Thread.Sleep(5000);
                            AnsiConsole.Markup($"Amount of Received messages : {report.ReceivedMessages.ToString()}\n");
                            Thread.Sleep(5000);
                            AnsiConsole.Markup($"Amount of Total messages : {report.TotalMessages.ToString()}\n");
                            Thread.Sleep(5000);
                            break;
                    }

                    break;
            }
        }
    }
}