using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class UserProfile
    {
        public string? Id { get; set; } // Nullable
        public string? About { get; set; } // Nullable
        public int Age { get; set; }
        public string? Gender { get; set; } // Nullable
        public string? City { get; set; } // Nullable
        public List<string> CanTeach { get; set; } = new List<string>();
        public List<string> WantToLearn { get; set; } = new List<string>();
        public bool HasSubscription { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int TotalCollaborations { get; set; }
    }

    public class CollaborationRequest
    {
        public int Id { get; set; }
        public string? SenderId { get; set; } // Nullable
        public string? ReceiverId { get; set; } // Nullable
        public List<string> TeachDirections { get; set; } = new List<string>();
        public List<string> LearnDirections { get; set; } = new List<string>();
        public CollaborationStatus Status { get; set; }
        public CollaborationType? Type { get; set; } // Already nullable
    }

    public enum CollaborationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum CollaborationType
    {
        Good,
        Barter
    }

    public class Subscription
    {
        public int Id { get; set; }
        public string? UserId { get; set; } // Nullable
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
