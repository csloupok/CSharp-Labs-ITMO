using Reports.DAL.Models;

namespace Reports.DAL.Interfaces;

public interface IEmployeeRepository
{
    List<Employee> GetAll();
    Employee GetById(Guid id);
    void Add(Employee employee);
    void Delete(Guid id);
    Employee GetByEmail(string email);
}