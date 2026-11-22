using ComputerRepairShopManagement.Components.models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairShopManagement.Components
{
    // All code here was done by Zack Da Silva
    public static class Summary
    {
        public static void write(RepairRequest request, Customer customer, string path = @"..\..\..\summaries")
        {
            Directory.CreateDirectory(path);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string formattedDate = date.Split(" ")[0].Replace("-", "") + "_" + date.Split(" ")[1].Replace(":", "");
            string filename = "Invoice_" + request.RepairID + "_" + formattedDate + ".txt";
            string savePath = Path.Combine(path, filename);

            var rq = request.GetRepairSummary(date);
            File.WriteAllText(savePath, rq.outPutA + customer.GetCustomerSummary() + rq.outPutB);
        }
    }
}
