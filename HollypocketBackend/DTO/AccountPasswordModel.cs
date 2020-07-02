using System.ComponentModel.DataAnnotations;

namespace HollypocketBackend.Models
{
    public class AccoutPasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
