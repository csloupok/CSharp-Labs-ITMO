namespace Reports.DAL.Interfaces;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IMessageRepository MessageRepository { get; }
    ISourceRepository SourceRepository { get; }
    IReportRepository ReportRepository { get; }
    IAccountRepository AccountRepository { get; }
}