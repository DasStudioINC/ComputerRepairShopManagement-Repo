using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Windows;
using ComputerRepairShopManagement.Components.models;
using System.Data;
namespace ComputerRepairShopManagement.Components
{
    // All code here was done by Zack Da Silva
    public static class Backend
    {

        private static string connectionString = "Server=zackies\\SQLEXPRESS;Database=TechFixDB;Trusted_Connection=True;TrustServerCertificate=True;";
        
        public static List<Customer> SELECT_FROM_CUSTOMER(params string[] columns)
        {
            string tags = string.Join(",", columns);
            string query = $@"SELECT {tags} FROM dbo.Customers ORDER BY CUSTOMERID";

            int count = 0;
            List<Customer> returnData = new List<Customer>();
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer cus = new Customer();
                                cus.CustumerId = getIndex("CustomerId", columns) == -1 ? null : reader.GetInt32(getIndex("CustomerId", columns));
                                cus.FullName = getIndex("FullName", columns) == -1 ? null : reader.GetString(getIndex("FullName", columns));
                                cus.PhoneNumber = getIndex("Phone", columns) == -1 ? null : reader.GetString(getIndex("Phone", columns));
                                cus.email = getIndex("Email", columns) == -1 ? null : reader.GetString(getIndex("Email", columns));
                                count++;
                                returnData.Add(cus);
                                
                            }
                        }
                    }
                    
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                MessageBox.Show(ex.Message, "Database Error");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            return returnData;
        }

        public static List<RepairRequest> SELECT_FROM_REPAIRE_REQUEST(params string[] columns)
        {
            string tags = string.Join(",", columns);
            string query = $@"SELECT {tags} FROM dbo.RepairRequests AS r INNER JOIN dbo.Customers AS c ON c.CUSTOMERID = r.CUSTOMERID ORDER BY REPAIRID";

            int count = 0;
            List<RepairRequest> returnData = new List<RepairRequest>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RepairRequest request = new RepairRequest();
                                request.RepairID = getIndex("RepairId", columns) == -1 ? null : reader.GetInt32(getIndex("RepairId", columns));
                                request.CustomerName = getIndex("CustomerName", columns) == -1 ? null : reader.GetString(getIndex("CustomerName", columns));
                                request.DeviceType = getIndex("DeviceType", columns) == -1 ? null : reader.GetString(getIndex("DeviceType", columns));
                                request.ProblemDescription = getIndex("ProblemDescription", columns) == -1 ? null : reader.GetString(getIndex("ProblemDescription", columns));
                                request.RepairDate = getIndex("repairdate", columns) == -1 ? null : reader.GetDateTime(getIndex("repairdate", columns)).Date;
                                request.Status = getIndex("status", columns) == -1 ? null : reader.GetString(getIndex("status", columns));
                                request.ServiceCost = getIndex("Servicecost", columns) == -1 ? null : reader.GetDecimal(getIndex("Servicecost", columns));
                                count++;
                                returnData.Add(request);
                               
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                MessageBox.Show(ex.Message, "Database Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            return returnData;
        }


      

        private static int getIndex(string target, string[] array)
        {
            return array.ToList().FindIndex(e => e.Contains(target, StringComparison.OrdinalIgnoreCase));
        }
    }
}
