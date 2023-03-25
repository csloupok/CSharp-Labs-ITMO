using Reports.DAL.Utils;

namespace Reports.DAL.Models;

public class Message
{
    private Guid _id;
    private string _text;
    private DateTime _newDate;
    private DateTime? _receivedDate;
    private DateTime? _processedDate;
    private Guid _sourceId;
    private Guid _employeeId;
    private MessageState _state;

    public Message(string text, Guid sourceId, Guid employeeId, DateTime newDate = default)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DatabaseException("Text cannot be null or empty.");
        if (sourceId == Guid.Empty)
            throw new DatabaseException("Source ID cannot be empty.");
        if (employeeId == Guid.Empty)
            throw new DatabaseException("Employee ID cannot be empty.");
        if (newDate == default)
            _newDate = DateTime.Now;
        _id = Guid.NewGuid();
        _text = text;
        _newDate = newDate;
        _receivedDate = null;
        _processedDate = null;
        _sourceId = sourceId;
        _employeeId = employeeId;
        _state = MessageState.New;
    }

    public Guid Id => _id;
    public string Text => _text;
    public DateTime NewDate => _newDate;
    public DateTime? ReceivedDate => _receivedDate;
    public DateTime? ProcessedDate => _processedDate;
    public Guid SourceId => _sourceId;
    public Guid EmployeeId => _employeeId;
    public MessageState State => _state;

    public void ReceiveMessage(DateTime time = default)
    {
        if (time == default)
            _newDate = DateTime.Now;
        if (time > _newDate)
            throw new DatabaseException("Received date cannot be less than New date.");
        _state = MessageState.Received;
        _receivedDate = time;
    }

    public void ProcessMessage(DateTime time = default)
    {
        if (time == default)
            _newDate = DateTime.Now;
        if (time > _receivedDate || time > _newDate)
            throw new DatabaseException("Processed date cannot be less than New date or Received date.");
        _state = MessageState.Processed;
        _processedDate = time;
    }
}