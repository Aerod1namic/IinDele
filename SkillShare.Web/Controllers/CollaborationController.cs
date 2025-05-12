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
    public class CollaborationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CollaborationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var collaborations = await _context.Collaborations
                .Include(c => c.Initiator)
                .Include(c => c.Target)
                .Include(c => c.InitiatorTeachingSkills)
                .Include(c => c.InitiatorLearningSkills)
                .Where(c => c.InitiatorId == currentUser.Id || c.TargetId == currentUser.Id)
                .ToListAsync();

            var viewModel = new CollaborationListViewModel
            {
                ActiveCollaborations = collaborations
                    .Where(c => c.Status == CollaborationStatus.Accepted)
                    .Select(c => CreateCollaborationCardViewModel(c, currentUser.Id))
                    .ToList(),

                PendingRequests = collaborations
                    .Where(c => c.Status == CollaborationStatus.Pending)
                    .Select(c => CreateCollaborationCardViewModel(c, currentUser.Id))
                    .ToList(),

                CompletedCollaborations = collaborations
                    .Where(c => c.Status == CollaborationStatus.Completed)
                    .Select(c => CreateCollaborationCardViewModel(c, currentUser.Id))
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var collaboration = await GetCollaborationWithUsers(id);
            if (collaboration == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || collaboration.TargetId != currentUser.Id)
            {
                return Forbid();
            }

            collaboration.Status = CollaborationStatus.Accepted;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var collaboration = await GetCollaborationWithUsers(id);
            if (collaboration == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || collaboration.TargetId != currentUser.Id)
            {
                return Forbid();
            }

            collaboration.Status = CollaborationStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var collaboration = await GetCollaborationWithUsers(id);
            if (collaboration == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || (collaboration.InitiatorId != currentUser.Id && collaboration.TargetId != currentUser.Id))
            {
                return Forbid();
            }

            collaboration.Status = CollaborationStatus.Completed;
            collaboration.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Rate(int id)
        {
            var collaboration = await GetCollaborationWithUsers(id);
            if (collaboration == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var isInitiator = collaboration.InitiatorId == currentUser.Id;
            if (!isInitiator && collaboration.TargetId != currentUser.Id)
            {
                return Forbid();
            }

            var partnerName = isInitiator 
                ? $"{collaboration.Target.FirstName} {collaboration.Target.LastName}"
                : $"{collaboration.Initiator.FirstName} {collaboration.Initiator.LastName}";

            var viewModel = new RateCollaborationViewModel
            {
                CollaborationId = collaboration.Id,
                PartnerName = partnerName
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating(RateCollaborationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Rate", model);
            }

            var collaboration = await GetCollaborationWithUsers(model.CollaborationId);
            if (collaboration == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            if (collaboration.InitiatorId == currentUser.Id)
            {
                collaboration.InitiatorRating = model.Rating;
                collaboration.InitiatorFeedback = model.Feedback;
            }
            else if (collaboration.TargetId == currentUser.Id)
            {
                collaboration.TargetRating = model.Rating;
                collaboration.TargetFeedback = model.Feedback;
            }
            else
            {
                return Forbid();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<IActionResult> Request(InitiateCollaborationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("InitiateCollaboration", "Profiles", new { userId = model.TargetUserId });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var targetUser = await _context.Users.FindAsync(model.TargetUserId);
            if (targetUser == null)
            {
                return NotFound();
            }

            // Получаем выбранные навыки
            var teachingSkills = await _context.Skills
                .Where(s => model.SelectedTeachingSkillIds.Contains(s.Id))
                .ToListAsync();

            var learningSkills = await _context.Skills
                .Where(s => model.SelectedLearningSkillIds.Contains(s.Id))
                .ToListAsync();

            var collaboration = new Collaboration
            {
                InitiatorId = currentUser.Id,
                TargetId = targetUser.Id,
                Type = model.CollaborationType,
                Status = CollaborationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                InitiatorTeachingSkills = teachingSkills,
                InitiatorLearningSkills = learningSkills
            };

            _context.Collaborations.Add(collaboration);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<Collaboration?> GetCollaborationWithUsers(int id)
        {
            return await _context.Collaborations
                .Include(c => c.Initiator)
                .Include(c => c.Target)
                .Include(c => c.InitiatorTeachingSkills)
                .Include(c => c.InitiatorLearningSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        private static CollaborationCardViewModel CreateCollaborationCardViewModel(Collaboration collaboration, string currentUserId)
        {
            var isInitiator = collaboration.InitiatorId == currentUserId;
            var partner = isInitiator ? collaboration.Target : collaboration.Initiator;

            return new CollaborationCardViewModel
            {
                Id = collaboration.Id,
                PartnerName = $"{partner.FirstName} {partner.LastName}",
                PartnerId = partner.Id,
                IsInitiator = isInitiator,
                Type = collaboration.Type,
                Status = collaboration.Status,
                CreatedAt = collaboration.CreatedAt,
                TeachingSkills = isInitiator 
                    ? collaboration.InitiatorTeachingSkills.Select(s => s.Name).ToList()
                    : collaboration.InitiatorLearningSkills.Select(s => s.Name).ToList(),
                LearningSkills = isInitiator
                    ? collaboration.InitiatorLearningSkills.Select(s => s.Name).ToList()
                    : collaboration.InitiatorTeachingSkills.Select(s => s.Name).ToList(),
                CanRate = collaboration.Status == CollaborationStatus.Completed && 
                    ((isInitiator && !collaboration.InitiatorRating.HasValue) ||
                    (!isInitiator && !collaboration.TargetRating.HasValue)),
                MyRating = isInitiator ? collaboration.InitiatorRating : collaboration.TargetRating,
                MyFeedback = isInitiator ? collaboration.InitiatorFeedback : collaboration.TargetFeedback,
                PartnerRating = isInitiator ? collaboration.TargetRating : collaboration.InitiatorRating,
                PartnerFeedback = isInitiator ? collaboration.TargetFeedback : collaboration.InitiatorFeedback
            };
        }
    }
} 