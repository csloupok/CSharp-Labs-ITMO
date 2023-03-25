using Reports.DAL.Models;

namespace Reports.BLL.Interfaces;

public interface IReportService
{
    Report GetById(Guid id);
    List<Report> GetAll();
    void Add(Report report);
    void Delete(Guid id);
    Report GenerateReport(DateTime startDate, DateTime endDate);
    public List<Report> GetReportsOfPeriod(DateTime startDate, DateTime endDate);
}