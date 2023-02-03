using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Dto
{
    public class PersonChargeDto
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string Phone { get; set; }
        public string Dep { get; set; }
        public string Email { get; set; }
        public DateTime EntryTime { get; set; }
        public string CustomerName { get; set; }
    }
}
