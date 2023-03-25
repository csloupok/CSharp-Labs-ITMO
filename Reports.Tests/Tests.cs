using Reports.BLL.Services;
using Reports.DAL;
using Reports.DAL.Repositories;
using Xunit;

namespace Reports.Tests;

public class Tests
{
    [Fact]
    public void GenerateReportAndCheckAmountOfMessagesTest()
    {
        const int totalNumberOfMessages = 3;
        const int numberOfReceivedMessages = 2;
        const int numberOfProcessedMessages = 1;
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

        var accountForEmployee = accountService.Create("kek@kek.kek", "kek");
        accountService.Add(accountForEmployee);
        var employee = employeeService.Create("Kek", accountForEmployee);
        employeeService.Add(employee);
        var accountForSource = accountService.Create("lol@lol.lol", "lol");
        accountService.Add(accountForSource);
        var source = sourceService.Create("Telegram", accountForSource);
        sourceService.Add(source);

        var message1 = messageService.Create("Kto prochital tot Employee", source.Id, employee.Id);
        messageService.Add(message1);
        var message2 = messageService.Create("Privet poshli v Dotu", source.Id, employee.Id);
        messageService.Add(message2);
        var message3 = messageService.Create("Uvolen", source.Id, employee.Id);
        messageService.Add(message3);

        messageService.ReceiveMessage(message1.Id);
        messageService.ReceiveMessage(message2.Id);
        messageService.ProcessMessage(message2.Id);

        var report = reportService.GenerateReport(DateTime.MinValue, DateTime.MaxValue);
        var numberOfMessagesForEmployee = report.TotalMessages;
        var receivedMessagesCount = report.ReceivedMessages;
        var processedMessagesCount = report.ProcessedMessages;

        Assert.Equal(totalNumberOfMessages, numberOfMessagesForEmployee);
        Assert.Equal(numberOfReceivedMessages, receivedMessagesCount);
        Assert.Equal(numberOfProcessedMessages, processedMessagesCount);
    }

    [Fact]
    public void EmployeeHierarchy()
    {
        const int numberOfSubordinates = 2;
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

        var accountForEmployee1 = accountService.Create("kek@kek.kek", "kek");
        accountService.Add(accountForEmployee1);
        var employee1 = employeeService.Create("Kek", accountForEmployee1);
        employeeService.Add(employee1);

        var accountForEmployee2 = accountService.Create("kek@kek.kek", "kek");
        accountService.Add(accountForEmployee2);
        var employee2 = employeeService.Create("Kek", accountForEmployee2);
        employeeService.Add(employee2);

        var accountForEmployee3 = accountService.Create("kek@kek.kek", "kek");
        accountService.Add(accountForEmployee3);
        var employee3 = employeeService.Create("Kek", accountForEmployee3);
        employeeService.Add(employee3);

        employeeService.AddSubordinate(employee1.Id, employee2.Id);
        employeeService.AddSubordinate(employee1.Id, employee3.Id);
        employeeService.ChangeSupervisor(employee2.Id, employee1.Id);
        employeeService.ChangeSupervisor(employee3.Id, employee1.Id);

        Assert.Equal(employee2.SupervisorId, employee1.Id);
        Assert.Equal(employee3.SupervisorId, employee1.Id);
        Assert.Equal(numberOfSubordinates, employee1.SubordinateIds.Count);
    }
}