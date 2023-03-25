namespace Reports.BLL.Entities;

public class Report
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid EmployeeId { get; set; }
    public int TotalMessages { get; set; }
    public int ProcessedMessages { get; set; }
    public Dictionary<Guid, int> DeviceStatistics { get; set; }
    public Dictionary<Guid, int> EmployeeStatistics { get; set; }
}