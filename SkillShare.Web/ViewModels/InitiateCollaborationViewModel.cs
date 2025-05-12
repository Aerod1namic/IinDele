using System.ComponentModel.DataAnnotations;
using SkillShare.Web.Models;

namespace SkillShare.Web.ViewModels
{
    public class InitiateCollaborationViewModel
    {
        public string TargetUserId { get; set; } = string.Empty;
        public string TargetUserName { get; set; } = string.Empty;

        public List<Skill> AvailableTeachingSkills { get; set; } = new();
        public List<Skill> AvailableLearningSkills { get; set; } = new();
        
        public List<Skill> TargetUserTeachingSkills { get; set; } = new();
        public List<Skill> TargetUserLearningSkills { get; set; } = new();

        [Required(ErrorMessage = "Выберите хотя бы один навык для обучения")]
        public List<int> SelectedTeachingSkillIds { get; set; } = new();

        [Required(ErrorMessage = "Выберите хотя бы один навык для изучения")]
        public List<int> SelectedLearningSkillIds { get; set; } = new();

        public CollaborationType CollaborationType { get; set; }
    }
} 