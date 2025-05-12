using System.ComponentModel.DataAnnotations;

namespace SkillShare.Web.ViewModels
{
    public class SkillViewModel
    {
        [Required(ErrorMessage = "Название навыка обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        public string Category { get; set; } = string.Empty;
    }
} 