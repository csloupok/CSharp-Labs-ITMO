namespace Reports.BLL.Entities;

public class Employee
{
    public Employee()
    {
        SubordinateIds = new List<Guid>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid? SupervisorId { get; set; }
    public List<Guid> SubordinateIds { get; set; }
}