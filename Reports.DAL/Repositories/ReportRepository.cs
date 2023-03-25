using Reports.DAL.Interfaces;
using Reports.DAL.Models;
using Reports.DAL.Utils;

namespace Reports.DAL.Repositories;

public class ReportRepository : IReportRepository
{
    private List<Report> _reports;

    public ReportRepository()
    {
        _reports = new List<Report>();
    }

    public List<Report> Reports => _reports;
    public List<Report> GetAll()
    {
        return _reports.ToList();
    }

    public Report GetById(Guid id)
    {
        return _reports.FirstOrDefault(e => e.Id == id) ?? throw new DatabaseException("Report is null.");
    }

    public void Add(Report report)
    {
        if (report is null)
            throw new DatabaseException("Report cannot be null.");
        if (_reports.Contains(report))
            throw new DatabaseException("Report already exists.");
        _reports.Add(report);
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        Report report = GetById(id);
        if (!_reports.Contains(report))
            throw new DatabaseException("Report doesn't exist.");
        _reports.Remove(report);
    }

    public List<Report> GetReportsByDateRange(DateTime startDate, DateTime endDate)
    {
        return _reports
            .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
            .ToList();
    }
}