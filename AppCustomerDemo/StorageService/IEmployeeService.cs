
namespace AppCustomerDemo.StorageService
{
    using AppCustomerDemo.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        Task AddEmployee(Employee employee);
        Task DeleteEmployee(string releaseYear, string title);
        Task<Employee> GetEmployee(string emplId, string rowKey);
        Task<List<Employee>> GetEmployeeList();
        Task UpdateEmployee(Employee employee);
    }
}