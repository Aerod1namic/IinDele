using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillShare.Web.Data;
using SkillShare.Web.Models;

namespace SkillShare.Web.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SubscriptionController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(SubscriptionType type)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var subscription = new Subscription
            {
                UserId = user.Id,
                StartDate = DateTime.UtcNow,
                EndDate = type switch
                {
                    SubscriptionType.Monthly => DateTime.UtcNow.AddMonths(1),
                    SubscriptionType.Quarterly => DateTime.UtcNow.AddMonths(3),
                    SubscriptionType.Yearly => DateTime.UtcNow.AddYears(1),
                    _ => DateTime.UtcNow.AddMonths(1)
                },
                Type = type
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            user.HasActiveSubscription = true;
            user.SubscriptionEndDate = subscription.EndDate;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Profile");
        }

        public async Task<IActionResult> Status()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var activeSubscription = await _context.Subscriptions
                .Where(s => s.UserId == user.Id && s.EndDate > DateTime.UtcNow)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefaultAsync();

            return View(activeSubscription);
        }
    }
} 