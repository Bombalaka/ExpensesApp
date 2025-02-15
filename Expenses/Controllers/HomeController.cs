using System.Diagnostics;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesDbContext _context;

        public HomeController(ILogger<HomeController> logger, ExpensesDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();
            var totalExpenses = allExpenses.Sum(expense => expense.Amount);
            ViewBag.Expenses = totalExpenses; 
            return View(allExpenses);
        }
        public IActionResult CreateEditExpenses(int? id)
        {
            if (id != null)
            {
                //editing -> load expenses by id 
                var expenseInId = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInId);
            }
            
            return View();
        }
        public IActionResult CreateEditExpensesForm(Expense model)
        {
            if (model.Id == 0)
            {
                //create
                _context.Expenses.Add(model);
            }
            else
            {
                //edit
                _context.Expenses.Update(model);
            }
            
            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }
        public IActionResult DeleteExpense(int id)
        {
            var expenseInId = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInId);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
