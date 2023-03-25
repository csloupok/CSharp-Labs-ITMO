using Reports.BLL.Interfaces;
using Reports.BLL.Utils;
using Reports.DAL.Interfaces;
using Reports.DAL.Models;

namespace Reports.BLL.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ApplicationLogicException("Unit of Work cannot be null.");
    }

    public List<Report> GetAll()
    {
        return _unitOfWork.ReportRepository.GetAll();
    }

    public Report GetById(Guid id)
    {
        return _unitOfWork.ReportRepository.GetById(id);
    }

    public void Add(Report report)
    {
        _unitOfWork.ReportRepository.Add(report);
    }

    public void Delete(Guid id)
    {
        _unitOfWork.ReportRepository.Delete(id);
    }

    public Report GenerateReport(DateTime startDate, DateTime endDate)
    {
        int totalMessages = _unitOfWork.MessageRepository.GetTotalMessagesCount(startDate, endDate);
        int receivedMessages = _unitOfWork.MessageRepository.GetReceivedMessagesCount(startDate, endDate);
        int processedMessages = _unitOfWork.MessageRepository.GetProcessedMessagesCount(startDate, endDate);

        Report report = new Report(startDate, endDate, totalMessages, receivedMessages, processedMessages);
        _unitOfWork.ReportRepository.Add(report);
        
        return report;
    }

    public List<Report> GetReportsOfPeriod(DateTime startDate, DateTime endDate)
    {
        return _unitOfWork.ReportRepository.GetReportsByDateRange(startDate, endDate);
    }
}