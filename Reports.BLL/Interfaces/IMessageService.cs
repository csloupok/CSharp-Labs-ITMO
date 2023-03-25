using Reports.DAL.Models;

namespace Reports.BLL.Interfaces;

public interface IMessageService
{
    Message GetById(Guid id);
    IReadOnlyList<Message> GetByEmployeeId(Guid employeeId);
    List<Message> GetAll();
    void Add(Message message);
    void Delete(Guid id);
    Message Create(string text, Guid sourceId, Guid employeeId, DateTime newDate);
    void ReceiveMessage(Guid messageId);
    void ProcessMessage(Guid messageId);
    int GetTotalMessagesCount(DateTime startDate, DateTime endDate);
    int GetReceivedMessagesCount(DateTime startDate, DateTime endDate);
    int GetProcessedMessagesCount(DateTime startDate, DateTime endDate);
}