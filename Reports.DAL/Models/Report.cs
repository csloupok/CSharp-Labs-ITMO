using Reports.DAL.Utils;

namespace Reports.DAL.Models;

public class Report
{
    private const int MinAmountOfMessages = 0;
    private Guid _id;
    private DateTime _startDate;
    private DateTime _endDate;
    private Guid? _employeeId;
    private int _totalMessages;
    private int _receivedMessages;
    private int _processedMessages;

    public Report(DateTime startDate, DateTime endDate, int totalMessages, int receivedMessages, int processedMessages)
    {
        if (startDate > endDate)
            throw new DatabaseException("Incorrect date range.");
        if (totalMessages < MinAmountOfMessages || totalMessages < receivedMessages || totalMessages < processedMessages)
            throw new DatabaseException("Incorrect amount of Total messages.");
        if (receivedMessages < MinAmountOfMessages)
            throw new DatabaseException("Incorrect amount of Received messages.");
        if (processedMessages < MinAmountOfMessages)
            throw new DatabaseException("Incorrect amount of Processed messages.");
        _id = Guid.NewGuid();
        _startDate = startDate;
        _endDate = endDate;
        _employeeId = Guid.Empty;
        _totalMessages = totalMessages;
        _receivedMessages = receivedMessages;
        _processedMessages = processedMessages;
    }

    public Guid Id => _id;
    public DateTime StartDate => _startDate;
    public DateTime EndDate => _endDate;
    public Guid? EmployerId => _employeeId;
    public int TotalMessages => _totalMessages;
    public int ReceivedMessages => _receivedMessages;
    public int ProcessedMessages => _processedMessages;
}