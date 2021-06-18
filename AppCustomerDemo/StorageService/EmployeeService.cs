
namespace AppCustomerDemo.StorageService
{
    using AppCustomerDemo.Models;
    using Azure.Storage.Queues;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class EmployeeService : IEmployeeService
    {
        private readonly IAzureTableStorage<Employee> repository;
        private QueueClient _queueClient;

        public EmployeeService(IAzureTableStorage<Employee> repository, QueueClient queueClient)
        {
            this.repository = repository;
            this._queueClient = queueClient;
        }

        public async Task AddEmployee(Employee employee)
        {
            //Create Message for Queue
            string message = JsonConvert.SerializeObject(employee);
            await _queueClient.SendMessageAsync(System.Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
            
            await this.repository.Insert(employee);
        }

        public async Task<Employee> GetEmployee(string partitionKey, string rowKey)
        {
            return await this.repository.GetItem(partitionKey, rowKey);
        }

        public async Task<List<Employee>> GetEmployeeList()
        {
            return await this.repository.GetList();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await this.repository.Update(employee);
        }

        public async Task DeleteEmployee(string partitionKey, string rowKey)
        {
            await this.repository.Delete(partitionKey, rowKey);
        }
    }
}