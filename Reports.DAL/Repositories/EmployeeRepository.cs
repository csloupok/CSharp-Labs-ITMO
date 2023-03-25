using Reports.DAL.Interfaces;
using Reports.DAL.Models;
using Reports.DAL.Utils;

namespace Reports.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employees;

    public EmployeeRepository()
    {
        _employees = new List<Employee>();
    }

    public List<Employee> Employees => _employees;

    public List<Employee> GetAll()
    {
        return _employees.ToList();
    }

    public Employee GetById(Guid id)
    {
        return _employees.FirstOrDefault(e => e.Id == id) ?? throw new DatabaseException("Employee is null.");
    }

    public void Add(Employee employee)
    {
        if (employee is null)
            throw new DatabaseException("Employee cannot be null.");
        if (_employees.Contains(employee))
            throw new DatabaseException("Employee already exists.");
        _employees.Add(employee);
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new DatabaseException("Id cannot be empty.");
        Employee employee = GetById(id);
        if (!_employees.Contains(employee))
            throw new DatabaseException("Employee doesn't exist.");
        _employees.Remove(employee);
    }

    public Employee GetByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DatabaseException("Email cannot be null or empty.");
        return _employees.FirstOrDefault(e => e.Account.Email == email) ??
               throw new DatabaseException("Employee is null.");
    }
}