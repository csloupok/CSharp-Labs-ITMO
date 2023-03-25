using Reports.DAL.Interfaces;
using Reports.DAL.Utils;

namespace Reports.DAL;

public class UnitOfWork : IUnitOfWork
{
    private IEmployeeRepository _employeeRepository;
    private IMessageRepository _messageRepository;
    private ISourceRepository _sourceRepository;
    private IReportRepository _reportRepository;
    private IAccountRepository _accountRepository;


    public UnitOfWork(IEmployeeRepository employeeRepository, IMessageRepository messageRepository, ISourceRepository sourceRepository, IReportRepository reportRepository, IAccountRepository accountRepository)
    {
        _employeeRepository = employeeRepository ?? throw new DatabaseException("Employee repository is null.");
        _messageRepository = messageRepository ?? throw new DatabaseException("Message repository is null.");
        _sourceRepository = sourceRepository ?? throw new DatabaseException("Source repository is null.");
        _reportRepository = reportRepository ?? throw new DatabaseException("Report repository is null.");
        _accountRepository = accountRepository ?? throw new DatabaseException("Account repository is null.");
    }

    public IEmployeeRepository EmployeeRepository => _employeeRepository;
    public IMessageRepository MessageRepository => _messageRepository;
    public ISourceRepository SourceRepository => _sourceRepository;
    public IReportRepository ReportRepository => _reportRepository;
    public IAccountRepository AccountRepository => _accountRepository;
}