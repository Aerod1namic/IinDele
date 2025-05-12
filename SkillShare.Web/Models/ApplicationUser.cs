using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SkillShare.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public int Age { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        public string? AboutMe { get; set; }

        public List<Skill> TeachingSkills { get; set; } = new();
        public List<Skill> LearningSkills { get; set; } = new();

        public int PositiveRatings { get; set; }
        public int NegativeRatings { get; set; }
        public int TotalCollaborations { get; set; }

        public bool HasActiveSubscription { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
} 