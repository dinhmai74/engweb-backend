using System.Collections.Generic;

namespace HollypocketBackend.Models
{
    public class AccountDto
    {
        public string Email { get; set; }
        public List<Address> Addresses { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public AccountType AccountType { get; set; }
        public string Id { get; set; }
    }
}
