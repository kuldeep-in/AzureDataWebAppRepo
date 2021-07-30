
namespace AppCustomerDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AppCustomerDemo.Models;
    using AppCustomerDemo.SQLService;
    using System.Linq;
    using System.Threading.Tasks;

    public class SQLController : Controller
    {
        private readonly SQLDBContext _dbContext;
        public SQLController(SQLDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ActionName("Index")]
        public IActionResult Index()
        {
            return View(_dbContext.DataPortal_Orders.ToList());
        }

        public IActionResult PowerBIPOC()
        {
            return View();
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
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDesc,Quantity,OrderValue")] Order order)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(order);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Order order = await _dbContext.DataPortal_Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("OrderId,OrderDesc,Quantity,OrderValue")] Order order)
        {
            if (ModelState.IsValid)
            {
                _dbContext.DataPortal_Orders.Update(order);  // .UpdateItemAsync(item.Id, item);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Order order = await _dbContext.DataPortal_Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAsync([Bind("OrderId")] long id)
        {
            var Order = new Order { OrderId = id };
            //_dbContext.Entry(Order).State = EntityState.Deleted;
            _dbContext.Remove(Order);
            _dbContext.SaveChanges();
            //await _dbContext.Orders.Remove(id);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(long id)
        {
            return View(await _dbContext.DataPortal_Orders.FindAsync(id));
        }
    }
}
