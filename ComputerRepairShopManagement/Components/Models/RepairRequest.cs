using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairShopManagement.Components.models
{
    public class RepairRequest
    {
        public int? RepairID { get; set; }
        public string? CustomerName { get; set; }
        public string? DeviceType { get; set; }

        public string? ProblemDescription { get; set; }

      
        public DateTime? RepairDate { get; set; }
        public string? Status { get; set; }
        public decimal? ServiceCost { get; set; }
    }
}
