using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManagement
{
    public class TenantItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string PropertyId { get; set; }
        public string LeaseStartDate { get; set; }
        public string LeaseEndDate { get; set; }
        public double RentAmount { get; set; }
        public string RentPaymentFrequency { get; set; }
        public bool IsActiveTenant { get; set; }
        public string ContractFile { get; set; }
    }
}
