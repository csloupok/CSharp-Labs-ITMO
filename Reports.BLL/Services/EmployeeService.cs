using Reports.BLL.Interfaces;
using Reports.BLL.Utils;
using Reports.DAL.Interfaces;
using Reports.DAL.Models;

namespace Reports.BLL.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ApplicationLogicException("Unit of Work cannot be null.");
    }

    public Employee GetById(Guid id)
    {
        return _unitOfWork.EmployeeRepository.GetById(id);
    }

    public Employee GetByEmail(string email)
    {
        return _unitOfWork.EmployeeRepository.GetByEmail(email);
    }

    public List<Employee> GetAll()
    {
        return _unitOfWork.EmployeeRepository.GetAll();
    }

    public void Add(Employee employee)
    {
        _unitOfWork.EmployeeRepository.Add(employee);
    }

    public void Delete(Guid id)
    {
        _unitOfWork.EmployeeRepository.Delete(id);
    }

    public Employee Create(string name, Account account)
    {
        return new Employee(name, account);
    }

    public void AddSubordinate(Guid employeeId, Guid subordinateId)
    {
        Employee employee = _unitOfWork.EmployeeRepository.GetById(employeeId);
        Employee subordinate = _unitOfWork.EmployeeRepository.GetById(subordinateId);

        if (employee == null || subordinate == null || employee.SubordinateIds.Contains(subordinateId))
        {
            throw new Exception("Invalid employee or subordinate.");
        }

        employee.AddSubordinate(subordinateId);
    }

    public void RemoveSubordinate(Guid employeeId, Guid subordinateId)
    {
        Employee employee = _unitOfWork.EmployeeRepository.GetById(employeeId);
        Employee subordinate = _unitOfWork.EmployeeRepository.GetById(subordinateId);

        if (employee == null || subordinate == null || !employee.SubordinateIds.Contains(subordinateId))
        {
            throw new Exception("Invalid employee or subordinate.");
        }

        employee.RemoveSubordinate(subordinateId);
    }

    public void ChangeSupervisor(Guid employeeId, Guid supervisorId)
    {
        Employee employee = _unitOfWork.EmployeeRepository.GetById(employeeId);
        Employee supervisor = _unitOfWork.EmployeeRepository.GetById(supervisorId);

        if (employee == null || supervisor == null || !supervisor.SubordinateIds.Contains(employeeId) || employee.SupervisorId == supervisorId)
        {
            throw new Exception("Invalid employee or supervisor.");
        }

        employee.ChangeSupervisor(supervisorId);
    }
}