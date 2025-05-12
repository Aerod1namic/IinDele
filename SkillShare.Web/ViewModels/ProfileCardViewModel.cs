using SkillShare.Web.Models;

namespace SkillShare.Web.ViewModels
{
    public class ProfileCardViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? AboutMe { get; set; }
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;
        public List<Skill> TeachingSkills { get; set; } = new();
        public List<Skill> LearningSkills { get; set; } = new();
        public int PositiveRatings { get; set; }
        public int NegativeRatings { get; set; }
        public int TotalCollaborations { get; set; }
        public bool HasActiveSubscription { get; set; }
    }

    public class ProfileSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? City { get; set; }
        public string? TeachingSkill { get; set; }
        public string? LearningSkill { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public List<ProfileCardViewModel> Profiles { get; set; } = new();
    }
} 