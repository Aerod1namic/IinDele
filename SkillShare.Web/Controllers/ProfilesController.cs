using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillShare.Web.Data;
using SkillShare.Web.Models;
using SkillShare.Web.ViewModels;

namespace SkillShare.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(ProfileSearchViewModel searchModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var query = _context.Users.AsQueryable();

            if (currentUser != null)
            {
                query = query.Where(u => u.Id != currentUser.Id);
            }

            // Применяем фильтры поиска
            if (!string.IsNullOrEmpty(searchModel.SearchTerm))
            {
                query = query.Where(u => 
                    (u.FirstName != null && u.FirstName.Contains(searchModel.SearchTerm)) || 
                    (u.LastName != null && u.LastName.Contains(searchModel.SearchTerm)) ||
                    (u.AboutMe != null && u.AboutMe.Contains(searchModel.SearchTerm)));
            }

            if (!string.IsNullOrEmpty(searchModel.City))
            {
                query = query.Where(u => u.City != null && u.City.Contains(searchModel.City));
            }

            if (searchModel.MinAge.HasValue)
            {
                query = query.Where(u => u.Age >= searchModel.MinAge.Value);
            }

            if (searchModel.MaxAge.HasValue)
            {
                query = query.Where(u => u.Age <= searchModel.MaxAge.Value);
            }

            // Загружаем связанные навыки
            query = query
                .Include(u => u.TeachingSkills)
                .Include(u => u.LearningSkills);

            if (!string.IsNullOrEmpty(searchModel.TeachingSkill))
            {
                query = query.Where(u => u.TeachingSkills.Any(s => s.Name.Contains(searchModel.TeachingSkill)));
            }

            if (!string.IsNullOrEmpty(searchModel.LearningSkill))
            {
                query = query.Where(u => u.LearningSkills.Any(s => s.Name.Contains(searchModel.LearningSkill)));
            }

            var users = await query.ToListAsync();

            // Преобразуем в ViewModel
            searchModel.Profiles = users.Select(u => new ProfileCardViewModel
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                AboutMe = u.AboutMe,
                Age = u.Age,
                City = u.City,
                TeachingSkills = u.TeachingSkills.ToList(),
                LearningSkills = u.LearningSkills.ToList(),
                PositiveRatings = u.PositiveRatings,
                NegativeRatings = u.NegativeRatings,
                TotalCollaborations = u.TotalCollaborations,
                HasActiveSubscription = u.HasActiveSubscription
            }).ToList();

            return View(searchModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendMessage(string userId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            // Здесь будет логика отправки сообщения
            // TODO: Реализовать систему сообщений

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> InitiateCollaboration(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            // Загружаем навыки текущего пользователя
            await _context.Entry(currentUser)
                .Collection(u => u.TeachingSkills)
                .LoadAsync();

            await _context.Entry(currentUser)
                .Collection(u => u.LearningSkills)
                .LoadAsync();

            var targetUser = await _context.Users
                .Include(u => u.TeachingSkills)
                .Include(u => u.LearningSkills)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (targetUser == null)
            {
                return NotFound();
            }

            // Фильтруем навыки, которые совпадают с интересами обоих пользователей
            var matchedTeachingSkills = currentUser.TeachingSkills
                .Where(skill => targetUser.LearningSkills.Any(s => s.Id == skill.Id))
                .ToList();

            var matchedLearningSkills = currentUser.LearningSkills
                .Where(skill => targetUser.TeachingSkills.Any(s => s.Id == skill.Id))
                .ToList();

            var viewModel = new InitiateCollaborationViewModel
            {
                TargetUserId = userId,
                TargetUserName = $"{targetUser.FirstName} {targetUser.LastName}",
                AvailableTeachingSkills = matchedTeachingSkills,
                AvailableLearningSkills = matchedLearningSkills,
                TargetUserTeachingSkills = targetUser.TeachingSkills.ToList(),
                TargetUserLearningSkills = targetUser.LearningSkills.ToList()
            };

            return View(viewModel);
        }
    }
} 