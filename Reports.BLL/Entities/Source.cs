namespace Reports.BLL.Entities;

public class Source
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public List<Guid> AccountIds { get; set; }
}