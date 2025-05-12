using System.ComponentModel.DataAnnotations;

namespace SkillShare.Web.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public SubscriptionType Type { get; set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }

    public enum SubscriptionType
    {
        Monthly,
        Quarterly,
        Yearly
    }
} 