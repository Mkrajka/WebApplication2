using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly WebDbContext _context;

        public MembersController(WebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            // Filter members based on the user's ID and the search query
            var membersQuery = _context.Members
                .Where(m => m.UserId == user.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                membersQuery = membersQuery.Where(m =>
                    m.MemberFirstName.ToLower().Contains(search) ||
                    m.MemberLastName.ToLower().Contains(search) ||
                    m.MemberEmail.ToLower().Contains(search) ||
                    m.MemberPhoneNumber.Contains(search));
            }

            var members = membersQuery.ToList();

            return PartialView("_SearchResultsPartial", members);
        }


        public async Task<IActionResult> Index(string search)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Query 1: Total Fees Amount for the specific user's members
            decimal totalFeesAmount = _context.TransactionFees
                .Where(tf => tf.Member.UserId == user.Id)
                .Sum(tf => tf.Amount);


            // Filter members based on the user's ID
            var membersQuery = _context.Members
                .Where(m => m.UserId == user.Id);

            // Apply search filter if a search query is provided
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                membersQuery = membersQuery.Where(m =>
                    m.MemberFirstName.ToLower().Contains(search) ||
                    m.MemberLastName.ToLower().Contains(search) ||
                    m.MemberEmail.ToLower().Contains(search) ||
                    m.MemberPhoneNumber.Contains(search));
            }

            var members = await membersQuery.ToListAsync();

            // Retrieve entry logs for the specific user
            var entryLogs = _context.EntryLogs
                .Where(el => el.Member.UserId == user.Id)
                .ToList();

            // Create a DashboardViewModel instance and set its Members property
            var viewModel = new WebApplication1.Models.ViewModels.DashboardViewModel
            {
                Members = members,
                EntryLogs = entryLogs
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        public IActionResult Members()
        {
            var members = _context.Members.ToList();
            return View(members);
        }

        [HttpPost]
        public async Task<IActionResult> AddDummyData(string FirstName, string LastName, string Email, string PhoneNumber, decimal Amount, DateTime DatePaid)
        {
            // Get the currently logged-in user
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Create a new Member with the UserId set to the current user's Id
            var newMember = new Member
            {
                MemberFirstName = FirstName,
                MemberLastName = LastName,
                MemberEmail = Email,
                MemberPhoneNumber = PhoneNumber,
                MemberDateJoined = DatePaid,
                ApplicationUser = user,
            };

            // Create a new TransactionFee associated with the new member
            var newTransactionFee = new TransactionFee
            {
                Amount = Amount,
                DatePaid = DatePaid,
                Member = newMember,
            };

            // Add the new Member and TransactionFee to the database
            _context.Members.Add(newMember);
            _context.TransactionFees.Add(newTransactionFee);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a success page or back to the Members page
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddingCalories(int BreakfastCalories, int LunchCalories, int DinnerCalories, int SnackCalories, int MemberID)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            var member = await _context.Members.FindAsync(MemberID);

            var Calorie = new CalorieEntry
            {
                Date = DateTime.Today,
                BreakfastCalories = BreakfastCalories,
                LunchCalories = LunchCalories,
                DinnerCalories = DinnerCalories,
                SnackCalories = SnackCalories,
                Member = member
            };

            _context.CalorieEntries.Add(Calorie);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Members", new { id = MemberID });
        }

        [HttpPost]
        public async Task<IActionResult> AddingWeight(double Weight, int MemberID)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            var member = await _context.Members.FindAsync(MemberID);

            var newWeight = new WeightEntry
            {
                Date = DateTime.Today,
                Weight = Weight,
                Member = member
            };

            _context.WeightEntries.Add(newWeight);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Members", new { id = MemberID });
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry(int memberId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Get the selected member
            Member member = _context.Members
                .Include(m => m.EntryLog)
                .FirstOrDefault(m => m.UserId == user.Id && m.MemberID == memberId);

            if (member != null)
            {
                // Check if there is an existing entry within the past 2 hours
                DateTime twoHoursAgo = DateTime.Now.AddHours(-2);

                bool hasExistingEntry = member.EntryLog.Any(el => el.EntryDate >= twoHoursAgo);

                if (!hasExistingEntry)
                {
                    // Create a new EntryLog
                    EntryLog newEntryLog = new EntryLog
                    {
                        EntryDate = DateTime.Now,
                        RfidTag = "Adnasndjkash kj234 ekja",
                        Member = member
                    };

                    // Add the new EntryLog to the database
                    _context.EntryLogs.Add(newEntryLog);
                    _context.SaveChanges();

                    return RedirectToAction("Details", "Members", new { id = memberId });
                }
                else
                {
                    return RedirectToAction("Details", "Members", new { id = memberId });
                }
            }

            // Handle the case where the member is not found
            return RedirectToAction("Details", "Members", new { id = memberId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirmed(Member updatedMember)
        {
            var existingMember = _context.Members.Find(updatedMember.MemberID);

            if (existingMember == null)
            {
                return NotFound();
            }

            // Update the existing member's properties with the edited values
            existingMember.MemberFirstName = updatedMember.MemberFirstName;
            existingMember.MemberLastName = updatedMember.MemberLastName;
            existingMember.MemberEmail = updatedMember.MemberEmail;
            existingMember.MemberPhoneNumber = updatedMember.MemberPhoneNumber;
            existingMember.MemberDateJoined = updatedMember.MemberDateJoined;
            existingMember.MemberDateLeft = updatedMember.MemberDateLeft;

            _context.Entry(existingMember).State = EntityState.Modified;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}
