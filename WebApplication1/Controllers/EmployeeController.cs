using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public EmployeeController(WebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            // Filter members based on the user's ID and the search query
            var employeesQuery = _context.Employees
                .Where(m => m.UserId == user.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                employeesQuery = employeesQuery.Where(m =>
                m.FirstName.ToLower().Contains(search) ||
                m.LastName.ToLower().Contains(search) ||
                m.DateOfBirth.ToString().Contains(search));
            }

            var employee = employeesQuery.ToList();

            return PartialView("searchResults", employee);
        }

        public async Task<IActionResult> Index(string search)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Filter members based on the user's ID and the search query
            var employeesQuery = _context.Employees
                .Where(m => m.UserId == user.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                employeesQuery = employeesQuery.Where(m =>
                    m.FirstName.ToLower().Contains(search) ||
                    m.LastName.ToLower().Contains(search) ||
                    m.DateOfBirth.ToString().Contains(search) ||
                    m.Gender.ToString().Contains(search) ||
                    m.Address.ToLower().Contains(search) ||
                    m.PhoneNumber.ToLower().Contains(search));
            }

            var employee = employeesQuery.ToList();

            return View(employee);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddEmployee(String FirstName, String LastName, DateTime DateOfBirth, Gender Gender, String Address, String PhoneNumber, String Email)
        {

            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Create a new resource instance
            var newEmployees = new Employee
            {
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Address = Address,
                PhoneNumber = PhoneNumber,
                Email = Email,
                ApplicationUser = user
            };

            // Add the new resource to the DbSet and save changes
            _context.Employees.Add(newEmployees);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirmed(Employee editedEmployee)
        {
            var originalEmployee = _context.Employees.Find(editedEmployee.EmployeeID);

            if (originalEmployee == null)
            {
                // If the member is not found, return a 404 Not Found result
                return NotFound();
            }

            originalEmployee.FirstName = editedEmployee.FirstName;
            originalEmployee.LastName = editedEmployee.LastName;
            originalEmployee.DateOfBirth = editedEmployee.DateOfBirth;
            originalEmployee.Gender = editedEmployee.Gender;
            originalEmployee.Address = editedEmployee.Address;
            originalEmployee.PhoneNumber = editedEmployee.PhoneNumber;
            originalEmployee.Email = editedEmployee.Email;

            _context.Entry(originalEmployee).State = EntityState.Modified;

            // Save changes to the database
            _context.SaveChanges();

            // If the model state is not valid, return to the edit view with validation errors
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'WebDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeID == id)).GetValueOrDefault();
        }
    }
}