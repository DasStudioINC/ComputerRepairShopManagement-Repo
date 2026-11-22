using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairShopManagement.Components
{
    // All code here was done by Zack Da Silva
    public class Customer
    {
        public int? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? email { get; set; }


        public string GetCustomerSummary()
        {
            return
                "CUSTOMER INFORMATION" +
                "\n------------------------" +
                $"\nName:  {FullName}" +
                $"\nPhone: {PhoneNumber}" +
                $"\nEmail: {email}\n\n";
        }
       
    }
}
