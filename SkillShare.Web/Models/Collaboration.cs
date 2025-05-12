using System.ComponentModel.DataAnnotations;

namespace SkillShare.Web.Models
{
    public enum CollaborationStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed,
        Cancelled
    }

    public class Collaboration
    {
        public int Id { get; set; }

        public string InitiatorId { get; set; } = string.Empty;
        public ApplicationUser Initiator { get; set; } = null!;

        public string TargetId { get; set; } = string.Empty;
        public ApplicationUser Target { get; set; } = null!;

        public CollaborationType Type { get; set; }
        public CollaborationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public List<Skill> InitiatorTeachingSkills { get; set; } = new();
        public List<Skill> InitiatorLearningSkills { get; set; } = new();

        [Range(0, 5)]
        public int? InitiatorRating { get; set; }
        public string? InitiatorFeedback { get; set; }

        [Range(0, 5)]
        public int? TargetRating { get; set; }
        public string? TargetFeedback { get; set; }
    }

    public enum CollaborationType
    {
        Barter
    }
} 