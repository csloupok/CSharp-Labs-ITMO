namespace Reports.BLL.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? SourceId { get; set; }
}