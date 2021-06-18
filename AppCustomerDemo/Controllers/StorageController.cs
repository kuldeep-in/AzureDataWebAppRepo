
namespace AppCustomerDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AppCustomerDemo.Models;
    using AppCustomerDemo.StorageService;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class StorageController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<StorageController> _logger;

        public StorageController(IEmployeeService employeeService, ILogger<StorageController> logger)
        {
            _service = employeeService;
            _logger = logger;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetEmployeeList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name,Employeeid,Location")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.RowKey = Guid.NewGuid().ToString();
                employee.PartitionKey = employee.Location;
                await _service.AddEmployee(employee);
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string partitionKey, string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey))
            {
                _logger.LogError("Row key is empty");
            }
            if (string.IsNullOrEmpty(partitionKey))
            {
                _logger.LogError("Partition key is empty");
            }
            return View(await _service.GetEmployee(partitionKey, rowKey));
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string partitionKey, string rowKey)
        {
            if (partitionKey == null)
            {
                return BadRequest();
            }

            Employee employee = await _service.GetEmployee(partitionKey, rowKey);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Name,Employeeid,Location,RowKey,PartitionKey")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string partitionKey, string rowKey)
        {
            if (partitionKey == null)
            {
                return BadRequest();
            }

            Employee employee = await _service.GetEmployee(partitionKey, rowKey);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("PartitionKey")] string partitionKey, [Bind("RowKey")] string rowKey)
        {
            await _service.DeleteEmployee(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

    }
}