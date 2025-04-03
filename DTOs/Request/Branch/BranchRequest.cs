using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch
{
    public class BranchRequest
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

       
    }
}
