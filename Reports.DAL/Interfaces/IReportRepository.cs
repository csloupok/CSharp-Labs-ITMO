using Reports.DAL.Models;

namespace Reports.DAL.Interfaces;

public interface IReportRepository
{
    List<Report> GetAll();
    Report GetById(Guid id);
    void Add(Report report);
    void Delete(Guid id);
    List<Report> GetReportsByDateRange(DateTime startDate, DateTime endDate);
}