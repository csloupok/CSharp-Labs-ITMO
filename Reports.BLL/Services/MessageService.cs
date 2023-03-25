using Reports.BLL.Interfaces;
using Reports.BLL.Utils;
using Reports.DAL.Interfaces;
using Reports.DAL.Models;

namespace Reports.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public MessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ApplicationLogicException("Unit of Work cannot be null.");
    }

    public Message GetById(Guid id)
    {
        return _unitOfWork.MessageRepository.GetById(id);
    }

    public IReadOnlyList<Message> GetByEmployeeId(Guid employeeId)
    {
        return _unitOfWork.MessageRepository.GetByEmployeeId(employeeId);
    }

    public List<Message> GetAll()
    {
        return _unitOfWork.MessageRepository.GetAll();
    }

    public void Add(Message message)
    {
        _unitOfWork.MessageRepository.Add(message);
    }

    public void Delete(Guid id)
    {
        _unitOfWork.MessageRepository.Delete(id);
    }

    public Message Create(string text, Guid sourceId, Guid employeeId, DateTime newDate = default)
    {
        return new Message(text, sourceId, employeeId, newDate);
    }
    
    public void ReceiveMessage(Guid messageId)
    {
        Message message = _unitOfWork.MessageRepository.GetById(messageId);

        if (message == null || message.State is MessageState.Processed or MessageState.Received)
            throw new Exception("Invalid message.");

        message.ReceiveMessage();
    }


    public void ProcessMessage(Guid messageId)
    {
        Message message = _unitOfWork.MessageRepository.GetById(messageId);

        if (message == null || message.State == MessageState.Processed)
            throw new Exception("Invalid message.");

        message.ProcessMessage();
    }

    public int GetTotalMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _unitOfWork.MessageRepository.GetTotalMessagesCount(startDate, endDate);
    }

    public int GetReceivedMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _unitOfWork.MessageRepository.GetReceivedMessagesCount(startDate, endDate);
    }

    public int GetProcessedMessagesCount(DateTime startDate, DateTime endDate)
    {
        return _unitOfWork.MessageRepository.GetProcessedMessagesCount(startDate, endDate);
    }
}