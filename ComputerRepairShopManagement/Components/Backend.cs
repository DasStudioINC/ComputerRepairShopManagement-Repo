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
        
        public static List<Customer> SELECT_FROM_CUSTOMER()
        {
            string query = @"SELECT * FROM dbo.Customers ORDER BY CUSTOMERID";

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
                                cus.CustomerId = reader.GetInt32(0);
                                cus.FullName = reader.GetString(1);
                                cus.PhoneNumber = reader.GetString(2);
                                cus.email = reader.GetString(3);
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

        public static Customer SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(int id = 1)
        {
            string query = $@"SELECT * FROM dbo.Customers WHERE CUSTOMERID = @CUSTOMERID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CUSTOMERID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Customer cus = new Customer();
                                cus.CustomerId = reader.GetInt32(0);
                                cus.FullName = reader.GetString(1);
                                cus.PhoneNumber = reader.GetString(2);
                                cus.email = reader.GetString(3);

                                return cus;
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

            return new Customer();
        }

        public static List<RepairRequest> SELECT_FROM_REPAIRE_REQUEST()
        {
            string query = @"SELECT r.RepairId, c.FullName, r.DeviceType, 
                        r.ProblemDescription, r.RepairDate , r.Status, r.ServiceCost
                        FROM dbo.RepairRequests AS r INNER JOIN dbo.Customers AS c 
                        ON c.CUSTOMERID = r.CUSTOMERID ORDER BY REPAIRID";

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
                                request.RepairID = reader.GetInt32(0);
                                request.CustomerName = reader.GetString(1);
                                request.DeviceType = reader.GetString(2);
                                request.ProblemDescription = reader.GetString(3);
                                request.RepairDate = reader.GetDateTime(4).Date;
                                request.Status = reader.GetString(5);
                                request.ServiceCost = reader.GetDecimal(6);
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

        public static RepairRequest SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(int id = 1)
        {
            string query = @"SELECT r.RepairId, c.FullName, r.DeviceType, 
                        r.ProblemDescription, r.RepairDate , r.Status, r.ServiceCost
                        FROM dbo.RepairRequests AS r INNER JOIN dbo.Customers AS c 
                        ON c.CUSTOMERID = r.CUSTOMERID WHERE r.RepairID = @REPAIRID ORDER BY REPAIRID ";

            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@REPAIRID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                RepairRequest request = new RepairRequest();
                                request.RepairID = reader.GetInt32(0);
                                request.CustomerName = reader.GetString(1);
                                request.DeviceType = reader.GetString(2);
                                request.ProblemDescription = reader.GetString(3);
                                request.RepairDate = reader.GetDateTime(4).Date;
                                request.Status = reader.GetString(5);
                                request.ServiceCost = reader.GetDecimal(6);
                                return request;
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

            return new RepairRequest();
        }


        /// <summary>
        /// Method implementation of inserting into the customers table.
        /// It does not take in a CustomerId because the creation of the 
        /// table Customer declared it to be Identity.
        /// </summary>
        /// <returns>The column that is converted into a Customer Object.</returns>
        public static Customer INSERT_INTO_CUSTOMER(string name, string phone, string email)
        {
            string query = @"INSERT INTO dbo.Customers
                (FULLNAME, PHONE, EMAIL) VALUES (@FULLNAME, @PHONE, @EMAIL)";

            
            int count = 0;
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FULLNAME", name);
                        cmd.Parameters.AddWithValue("@PHONE", phone);
                        cmd.Parameters.AddWithValue("@EMAIL", email);

                        connection.Open();
                        count = cmd.ExecuteNonQuery();
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

            Customer cus = new Customer();
            cus.CustomerId = count;
            cus.FullName = name;
            cus.email = email;
            cus.PhoneNumber = phone;

            return cus;
        }

        //public static (Customer a, Customer b) UPDATE_CUSTOMER_COLUMN(int id, string? name, string? phone, string? email) 
        //{

        //}

        private static int getIndex(string target, string[] array)
        {
            return array.ToList().FindIndex(e => e.Contains(target, StringComparison.OrdinalIgnoreCase));
        }
    }
}
