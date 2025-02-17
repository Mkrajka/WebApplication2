using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebDbContext _context;
        public DashboardController(WebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Query 1: Total Fees Amount for the specific user's members
            decimal totalFeesAmount = _context.TransactionFees
                .Where(tf => tf.Member.UserId == user.Id)
                .Sum(tf => tf.Amount);

            // Query 2: Count of New Members who joined in the recent one month for the specific user
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            int newMembersCount = _context.Members
                .Count(m => m.MemberDateJoined >= oneMonthAgo && m.UserId == user.Id);

            // Query 3: Count of Active Admission for the specific user (members with non-null MemberDateJoined)
            int activeAdmissionMembersCount = _context.Members
                .Count(m => m.MemberDateJoined != null && m.UserId == user.Id);

            // Retrieve the list of members for the specific user
            var members = _context.Members
                .Where(m => m.UserId == user.Id)
                .ToList();

            // Calculate the start date for the last seven days
            DateTime startDate = DateTime.Now.Date.AddDays(-6);

            // Create a list with default values (0) for the last seven days
            var entryLogsPerDay = Enumerable.Range(0, 7)
                .Select(offset => startDate.AddDays(offset).Date)
                .ToDictionary(date => date, date => 0);

            // Query 5: Retrieve EntryLogs for the last seven days for the specific user
            var entryLogsLastSevenDays = _context.EntryLogs
                .Where(el => el.Member.UserId == user.Id && el.EntryDate >= startDate)
                .ToList();

            // Update the counts in the entryLogsPerDay list
            foreach (var entryLog in entryLogsLastSevenDays)
            {
                var entryDate = entryLog.EntryDate.Date;
                entryLogsPerDay[entryDate]++;
            }

            // Query 6: Retrieve Members joined and left per month for the last 12 months
            DateTime startMonthDate = DateTime.Now.Date.AddMonths(-11); // Start from 12 months ago

            var currentDate = DateTime.Now.Date;

            // Create a sequence of the last 12 months
            var last12Months = Enumerable.Range(0, 12)
                .Select(offset => currentDate.AddMonths(-offset))
                .Select(date => new DateTime(date.Year, date.Month, 1))
                .ToList();

            // Query 6: Retrieve Members joined per month for the last 12 months
            var membersJoinedPerMonth = last12Months
                .GroupJoin(
                    _context.Members
                        .Where(m => m.UserId == user.Id && m.MemberDateJoined >= startMonthDate)
                        .GroupBy(m => new { Year = m.MemberDateJoined.Year, Month = m.MemberDateJoined.Month })
                        .Select(group => new EntryCount
                        {
                            Date = new DateTime(group.Key.Year, group.Key.Month, 1),
                            Count = group.Count()
                        }),
                    month => month,
                    joinedData => joinedData.Date,
                    (month, joinedData) => joinedData.SingleOrDefault() ?? new EntryCount { Date = month, Count = 0 }
                )
                .ToDictionary(entry => entry.Date, entry => entry);

            // Query 6: Retrieve Members left per month for the last 12 months
            var membersLeftPerMonth = last12Months
                .GroupJoin(
                    _context.Members
                        .Where(m => m.UserId == user.Id && m.MemberDateLeft.HasValue && m.MemberDateLeft.Value >= startMonthDate)
                        .GroupBy(m => new { Year = m.MemberDateLeft.Value.Year, Month = m.MemberDateLeft.Value.Month })
                        .Select(group => new EntryCount
                        {
                            Date = new DateTime(group.Key.Year, group.Key.Month, 1),
                            Count = group.Count()
                        }),
                    month => month,
                    leftData => leftData.Date,
                    (month, leftData) => leftData.SingleOrDefault() ?? new EntryCount { Date = month, Count = 0 }
                )
                .ToDictionary(entry => entry.Date, entry => entry);


            // Create a ViewModel to hold the data for the view
            var viewModel = new DashboardViewModel
            {
                TotalFeesAmount = totalFeesAmount,
                NewMembersCount = newMembersCount,
                ActiveAdmissionMembersCount = activeAdmissionMembersCount,
                Members = members,
                EntryLogsPerDay = entryLogsPerDay,
                MembersJoinedPerMonth = membersJoinedPerMonth,
                MembersLeftPerMonth = membersLeftPerMonth
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }

        public async Task<IActionResult> EntriesGraph()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Member member = await _context.Members
                .Include(m => m.EntryLog)
                .FirstOrDefaultAsync(m => m.UserId == user.Id);

            if (member != null)
            {
                var entriesPerDay = member.EntryLog
                    .Where(el => el.EntryDate >= DateTime.Today.AddMonths(-1))
                    .GroupBy(el => el.EntryDate.Date)
                    .Select(group => new EntryCount { Date = group.Key, Count = group.Count() })
                    .OrderBy(entry => entry.Date)
                    .ToList();

                var viewModel = new EntriesGraphViewModel
                {
                    EntriesPerDay = entriesPerDay
                };

                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
