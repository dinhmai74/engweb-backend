using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Models
{
    public class OrderInput
    {
        public string Note { get; set; }
        public string ReceiverAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string CashType { get; set; }
    }
}
