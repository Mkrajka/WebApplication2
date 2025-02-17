using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EmailsController : Controller
    {
        private readonly WebDbContext _context;
        private readonly EmailReceiver _emailReceiver;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailsController(WebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailReceiver = new EmailReceiver();
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string emailAddress)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Retrieve all members from the database
            var allMembers = await _context.Members.Where(m => m.UserId == user.Id).ToListAsync(); // Replace _context.Members with your actual DbSet for members
            ViewBag.AllMembers = allMembers;

            // Retrieve all employees from the database
            var allEmployees = await _context.Employees.Where(m => m.UserId == user.Id).ToListAsync(); // Replace _context.Employees with your actual DbSet for employees
            ViewBag.AllEmployees = allEmployees;

            // Retrieve all emails from the database
            var allEmails = await _context.Emails.Where(m => m.UserId == user.Id).ToListAsync(); // Replace _context.Emails with your actual DbSet for emails

            // Filter emails based on the entered email address
            /*var filteredEmails = allEmails.Where(e => e.SenderId == emailAddress || e.ReceiverId == emailAddress).ToList();*/
            var filteredEmails = allEmails.Where(e => e.SenderId == emailAddress).ToList();

            ViewBag.FilteredEmails = filteredEmails;

            // Create a new instance of Email if needed
            Email email = new Email(); // Assuming you're creating a new instance of Email

            return View(email);
        }

        public async Task<IActionResult> SentEmails(string receiverEmail)
        {
            // Filter sent emails by the receiver's email address
            var sentEmails = await _context.Emails
                .Where(e => e.ReceiverId == receiverEmail) 
                .ToListAsync();

            ViewBag.SentEmails = sentEmails;

            return View(sentEmails);
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedEmails(string emailAddress)
        {
            // Check if the email address is provided
            if (string.IsNullOrEmpty(emailAddress))
            {
                ModelState.AddModelError(string.Empty, "Email address is required.");
                return RedirectToAction(nameof(Index));
            }

            // Retrieve emails for the specified email address
            var receivedEmails = await _context.Emails
                .Where(e => e.ReceiverId == emailAddress)
                .ToListAsync();

            // Set ViewBag with received emails
            ViewBag.ReceivedEmails = receivedEmails;

            // Redirect to Index action
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string ReceiverId, string Subject, string Content)
        {
            // Sender's email address and app-specific password
            string fromAddress = "gymmanagement0@gmail.com";
            string password = "tsda oboq jsdv mjgq";

            // Get the currently logged-in user
            ApplicationUser user = await _userManager.GetUserAsync(User);

            try
            {
                // Set up SMTP client
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(fromAddress, password);

                // Create MailMessage object
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress);
                mailMessage.To.Add(new MailAddress(ReceiverId));
                mailMessage.Subject = Subject;
                mailMessage.Body = Content;

                // Send the email
                smtp.Send(mailMessage);

                // Create a new Member with the UserId set to the current user's Id
                var newEmail = new Email
                {
                    SenderId = fromAddress,
                    ReceiverId = ReceiverId,
                    Subject = Subject,
                    Content = Content,
                    Timestamp = DateTime.Now,
                    ApplicationUser = user,
                };

                // Add the new email to the database
                _context.Emails.Add(newEmail);

                // Save changes to the database
                await _context.SaveChangesAsync();

                ViewBag.Message = "Email sent successfully!";
            }
            catch (System.Exception ex)
            {
                ViewBag.Message = "Error sending email: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EmailPage()
        {
            var members = _context.Members.ToList(); 

            return View(members);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EmailId == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailId,SenderId,ReceiverId,Subject,Content,Timestamp,UserId")] Email email)
        {
            if (ModelState.IsValid)
            {
                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", email.UserId);
            return View(email);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", email.UserId);
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmailId,SenderId,ReceiverId,Subject,Content,Timestamp,UserId")] Email email)
        {
            if (id != email.EmailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.EmailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", email.UserId);
            return View(email);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EmailId == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emails == null)
            {
                return Problem("Entity set 'WebDbContext.Emails'  is null.");
            }
            var email = await _context.Emails.FindAsync(id);
            if (email != null)
            {
                _context.Emails.Remove(email);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(int id)
        {
          return (_context.Emails?.Any(e => e.EmailId == id)).GetValueOrDefault();
        }
    }
}
