namespace Reports.BLL.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid SourceId { get; set; }
    public Guid AccountId { get; set; }
    public MessageState State { get; set; }
}