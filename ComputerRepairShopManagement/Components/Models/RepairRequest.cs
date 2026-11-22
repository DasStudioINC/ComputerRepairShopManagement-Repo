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

        public (string outPutA, string outPutB) GetRepairSummary(string inv_gen)
        {
            string a = "TECHFIX SOLUTIONS" +
                "\nCOMPUTER REPAIR INVOICE" +
                "\n-----------------------------" +
                "\n\n" +
                $"Invoice Generated: {inv_gen}" +
                $"\nRepair ID:         {RepairID}\n\n";


            string b =
                "REPAIR DETAILS" +
                "\n------------------------" +
                $"\nDevice Type: {DeviceType}" +
                $"\nProblem:     {ProblemDescription}" +
                $"\nRepair Date: {RepairDate?.ToString("yyyy-MM-dd")}" +
                $"\nStatus:      {Status}\n" +
                $"\n-----------------------------" +
                $"\nTOTAL AMOUNT: ${ServiceCost}" +
                $"\n-----------------------------" +
                $"\n\n" +
                $"Thank you for choosing TechFix!";

            return (outPutA: a, outPutB: b);
        }
    }
}
