using Reports.DAL.Utils;

namespace Reports.DAL.Models;

public class Employee
{
    private Guid _id;
    private string _name;
    private Account _account;
    private Guid _supervisorId;
    private List<Guid> _subordinateIds;

    public Employee(string name, Account account)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DatabaseException("Name cannot be null or empty.");
        _id = Guid.NewGuid();
        _name = name;
        _account = account ?? throw new DatabaseException("Account cannot be null.");
        _supervisorId = Guid.Empty;
        _subordinateIds = new List<Guid>();
    }

    public Guid Id => _id;
    public string Name => _name;
    public Account Account => _account;
    public Guid SupervisorId => _supervisorId;
    public IReadOnlyList<Guid> SubordinateIds => _subordinateIds;

    public void ChangeSupervisor(Guid supervisorId)
    {
        if (supervisorId == Guid.Empty)
            throw new DatabaseException("Supervisor ID cannot be empty.");
        _supervisorId = supervisorId;
    }

    public void AddSubordinate(Guid subordinateId)
    {
        if (subordinateId == Guid.Empty)
            throw new DatabaseException("Subordinate ID cannot be empty.");
        if (_subordinateIds.Contains(subordinateId))
            throw new DatabaseException("Subordinate ID is already in list.");
        _subordinateIds.Add(subordinateId);
    }

    public void RemoveSubordinate(Guid subordinateId)
    {
        if (subordinateId == Guid.Empty)
            throw new DatabaseException("Subordinate ID cannot be empty.");
        if (!_subordinateIds.Contains(subordinateId))
            throw new DatabaseException("Subordinate ID is not in list.");
        _subordinateIds.Remove(subordinateId);
    }
}