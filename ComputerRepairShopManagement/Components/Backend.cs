using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Windows;
namespace ComputerRepairShopManagement.Components
{
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
                                Console.WriteLine($"{cus.CustumerId} - {cus.FullName} - {cus.PhoneNumber} - {cus.email}");
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


        private static int getIndex(string target, string[] array)
        {
            return array.ToList().FindIndex(e => e.Contains(target, StringComparison.OrdinalIgnoreCase));
        }
    }
}
