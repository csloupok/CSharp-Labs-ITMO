using Reports.DAL.Models;

namespace Reports.BLL.Interfaces;

public interface IEmployeeService
{
    Employee GetById(Guid id);
    Employee GetByEmail(string email);
    List<Employee> GetAll();
    void Add(Employee employee);
    void Delete(Guid id);
    Employee Create(string name, Account account);
    void AddSubordinate(Guid employeeId, Guid subordinateId);
    void RemoveSubordinate(Guid employeeId, Guid subordinateId);
    void ChangeSupervisor(Guid employeeId, Guid supervisorId);
}
