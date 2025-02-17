using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassesController(WebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            // Filter members based on the user's ID and the search query
            var resourcesQuery = _context.Classes
                .Where(m => m.UserId == user.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                resourcesQuery = resourcesQuery.Where(m =>
                    m.ClassName.ToLower().Contains(search));
            }

            var classes = resourcesQuery.ToList();

            return PartialView("searchResults", classes);
        }

        public async Task<IActionResult> Index(string search)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Filter classes based on the user's ID and the search query
            var classQuery = _context.Classes
                .Where(m => m.UserId == user.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                classQuery = classQuery.Where(m =>
                    m.ClassName.ToLower().Contains(search));
            }

            // Include the ClassMember and ClassEmployee navigation properties to eagerly load ClassMembers and ClassEmployees
            var classes = await classQuery
                .Include(c => c.ClassMember)
                .Include(c => c.ClassEmployee)
                .ToListAsync();

            // Populate dropdown lists
            ViewBag.MemberList = await _context.Members.ToListAsync();
            ViewBag.EmployeeList = await _context.Employees.ToListAsync();

            return View(classes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        public async Task<IActionResult> AddClassMembers(Classes Class, List<int> MemberIDs)
        {

            foreach (var memberID in MemberIDs)
            {
                // Fetch the employee object from the database using the employee ID
                var member = await _context.Members.FindAsync(memberID);

                if (member != null)
                {
                    var newClassEmployee = new ClassMember
                    {
                        Class = Class,
                        Member = member
                    };

                    // Add the new class employee to the database
                    _context.ClassMembers.Add(newClassEmployee);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a success page or back to the Classes page
            return RedirectToAction(nameof(Index));
        }



        
        [HttpPost]
        public async Task<IActionResult> AddClass(string ClassName, string Description, DateTime Date, TimeSpan Duration, List<int> MemberIDs, List<int> EmployeeIDs)
        {
            // Get the currently logged-in user
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Create a new Class with the UserId set to the current user's Id
            var newClass = new Classes
            {
                ClassName = ClassName,
                Description = Description,
                Date = Date,
                Duration = Duration,
                ApplicationUser = user,
            };

            // Add the new class to the database
            _context.Classes.Add(newClass);
          
            // Create ClassMember entries for each selected member
            foreach (var memberID in MemberIDs)
            {
                var member = await _context.Members.FindAsync(memberID);

                if (member != null)
                {
                    var newClassMember = new ClassMember
                    {
                        Class = newClass,
                        Member = member
                    };

                    // Add the new class member to the database
                    _context.ClassMembers.Add(newClassMember);
                }
            }


            // Create ClassEmployee entries for each selected employee
            foreach (var employeeID in EmployeeIDs)
            {
                // Fetch the employee object from the database using the employee ID
                var employee = await _context.Employees.FindAsync(employeeID);

                if (employee != null)
                {
                    var newClassEmployee = new ClassEmployee
                    {
                        Class = newClass,
                        Employee = employee
                    };

                    // Add the new class employee to the database
                    _context.ClassEmployees.Add(newClassEmployee);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a success page or back to the Classes page
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the class by its ID
            var @class = await _context.Classes
                .Include(c => c.ClassMember)
                .Include(c => c.ClassEmployee)
                .FirstOrDefaultAsync(c => c.ClassID == id);

            if (@class == null)
            {
                return NotFound(); 
            }

            // Populate dropdown lists for members and employees
            ViewBag.MemberList = await _context.Members.ToListAsync();
            ViewBag.EmployeeList = await _context.Employees.ToListAsync();

            return View(@class);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirmed(Classes editedClass)
        {
            var originalClass = _context.Classes.Find(editedClass.ClassID);

            if (originalClass == null)
            {
                // If the member is not found, return a 404 Not Found result
                return NotFound();
            }

            originalClass.ClassName = editedClass.ClassName;
            originalClass.Description = editedClass.Description;
            originalClass.Date = editedClass.Date;
            originalClass.Duration = editedClass.Duration;
            
            _context.Entry(originalClass).State = EntityState.Modified;

            // Save changes to the database
            _context.SaveChanges();

            // If the model state is not valid, return to the edit view with validation errors
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the class by its ID
            var classToDelete = await _context.Classes.FindAsync(id);
            if (classToDelete == null)
            {
                return NotFound();
            }

            try
            {
                // Remove associated class members
                var classMembers = _context.ClassMembers.Where(cm => cm.ClassID == id);
                _context.ClassMembers.RemoveRange(classMembers);

                // Remove associated class employees
                var classEmployees = _context.ClassEmployees.Where(ce => ce.ClassID == id);
                _context.ClassEmployees.RemoveRange(classEmployees);

                // Remove the class itself
                _context.Classes.Remove(classToDelete);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(500, ex.Message); 
            }
        }


        private bool ClassesExists(int id)
        {
          return (_context.Classes?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }
    }
}
