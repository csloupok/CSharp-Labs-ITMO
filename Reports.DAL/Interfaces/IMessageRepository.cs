using Reports.DAL.Models;

namespace Reports.DAL.Interfaces;

public interface IMessageRepository
{
    List<Message> GetAll();
    Message GetById(Guid id);
    IReadOnlyList<Message> GetByEmployeeId(Guid employeeId);
    void Add(Message message);
    void Delete(Guid id);
    int GetTotalMessagesCount(DateTime startDate, DateTime endDate);
    int GetReceivedMessagesCount(DateTime startDate, DateTime endDate);
    int GetProcessedMessagesCount(DateTime startDate, DateTime endDate);
}