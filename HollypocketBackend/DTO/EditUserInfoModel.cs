using System.ComponentModel.DataAnnotations;

namespace HollypocketBackend.Models
{
    public class EditUserInfoModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Address[] Addresses { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
