using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory
{
    public class AddTreatmentCategoryRequest
    {
        [Required]
        public string TreatmentName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
