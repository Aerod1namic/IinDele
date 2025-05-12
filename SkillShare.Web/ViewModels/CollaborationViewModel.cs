using SkillShare.Web.Models;

namespace SkillShare.Web.ViewModels
{
    public class CollaborationCardViewModel
    {
        public int Id { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public string PartnerId { get; set; } = string.Empty;
        public bool IsInitiator { get; set; }
        public CollaborationType Type { get; set; }
        public CollaborationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> TeachingSkills { get; set; } = new();
        public List<string> LearningSkills { get; set; } = new();
        public bool CanRate { get; set; }
        public int? MyRating { get; set; }
        public string? MyFeedback { get; set; }
        public int? PartnerRating { get; set; }
        public string? PartnerFeedback { get; set; }
    }

    public class CollaborationListViewModel
    {
        public List<CollaborationCardViewModel> ActiveCollaborations { get; set; } = new();
        public List<CollaborationCardViewModel> PendingRequests { get; set; } = new();
        public List<CollaborationCardViewModel> CompletedCollaborations { get; set; } = new();
    }

    public class RateCollaborationViewModel
    {
        public int CollaborationId { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Feedback { get; set; }
    }
} 