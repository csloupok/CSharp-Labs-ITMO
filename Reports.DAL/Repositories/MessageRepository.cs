using Reports.DAL.Interfaces;
using Reports.DAL.Models;
using Reports.DAL.Utils;

namespace Reports.DAL.Repositories;

public class MessageRepository : IMessageRepository
{
    private List<Message> _messages;

    public MessageRepository()
    {
        _messages = new List<Message>();
    }
    
    public List<Message> Messages => _messages;

    public List<Message> GetAll()
    {
        return _messages.ToList();
    }

    public Message GetById(Guid id)
    {
        return _messages.FirstOrDefault(e => e.Id == id) ?? throw new DatabaseException("Message is null.");
    }
    
    public IReadOnlyList<Message> GetByEmployeeId(Guid employeeId)
    {
        return _messages.FindAll(m => m.EmployeeId == employeeId);
    }

    public void Add(Message message)
    {
        if (message is null)
            throw new DatabaseException("Message cannot be null.");
        if (_messages.Contains(message))
            throw new DatabaseException("Message already exists.");
        _messages.Add(message);
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        Message message = GetById(id);
        if (!_messages.Contains(message))
            throw new DatabaseException("Message doesn't exist.");
        _messages.Remove(message);
    }

    public int GetTotalMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _messages.Count(m => m.NewDate >= startDate && m.NewDate <= endDate);
    }

    public int GetReceivedMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _messages.Count(m =>
            m.NewDate >= startDate && m.NewDate <= endDate && (m.State == MessageState.Received || m.State == MessageState.Processed));
    }

    public int GetProcessedMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _messages.Count(m =>
            m.NewDate >= startDate && m.NewDate <= endDate && m.State == MessageState.Processed);
    }
}