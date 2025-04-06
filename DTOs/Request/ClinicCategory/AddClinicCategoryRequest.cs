using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory
{
    public class AddClinicCategoryRequest
    {
        [Required]
        public string ClinicCategoryName { get; set; }
        public string DayOfTheWeek { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
