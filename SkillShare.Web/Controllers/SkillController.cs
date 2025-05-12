using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillShare.Web.Data;
using SkillShare.Web.Models;
using SkillShare.Web.ViewModels;

namespace SkillShare.Web.Controllers
{
    [Authorize]
    public class SkillController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var skills = await _context.Skills
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return View(skills);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToProfile(int skillId, string type)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var skill = await _context.Skills.FindAsync(skillId);
            if (skill == null)
            {
                return NotFound();
            }

            if (type == "teaching")
            {
                if (!user.TeachingSkills.Contains(skill))
                {
                    user.TeachingSkills.Add(skill);
                }
            }
            else if (type == "learning")
            {
                if (!user.LearningSkills.Contains(skill))
                {
                    user.LearningSkills.Add(skill);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromProfile(int skillId, string type)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var skill = await _context.Skills.FindAsync(skillId);
            if (skill == null)
            {
                return NotFound();
            }

            if (type == "teaching")
            {
                user.TeachingSkills.Remove(skill);
            }
            else if (type == "learning")
            {
                user.LearningSkills.Remove(skill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<Skill>());
            }

            var skills = await _context.Skills
                .Where(s => s.Name.Contains(query) || (s.Description != null && s.Description.Contains(query)))
                .Take(10)
                .Select(s => new { id = s.Id, text = s.Name })
                .ToListAsync();

            return Json(skills);
        }
    }
} 