using ComputerRepairShopManagement.Components.Enums;
using ComputerRepairShopManagement.Components.models;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Windows;
namespace ComputerRepairShopManagement.Components
{
    // All code here was done by Zack Da Silva
    public static class Backend
    {

        private static string connectionString = "Server=zackies\\SQLEXPRESS;Database=TechFixDB;Trusted_Connection=True;TrustServerCertificate=True;";







        // Customer 
        public static (DataTable table, List<Customer> customers) SELECT_FROM_CUSTOMER()
        {
            string query = @"SELECT CustomerID AS 'Customer ID', FULLNAME AS 'Full Name',
                             Phone, Email
                            FROM dbo.Customers ORDER BY CUSTOMERID";

            int count = 0;
            DataTable table = new DataTable();
            List<Customer> returnData = new List<Customer>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }


                    foreach (DataRow row in table.Rows)
                    {
                        Customer cus = new Customer();
                        cus.CustomerId = row.Field<int>("Customer ID");
                        cus.FullName = row.Field<string>("Full Name");
                        cus.PhoneNumber = row.Field<string>("Phone");
                        cus.email = row.Field<string>("Email");
                        count++;
                        returnData.Add(cus);
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


            return (table: table, customers: returnData);
        }

        public static (DataTable table, Customer customer) SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(int id = 1)
        {
            string query = $@"SELECT CustomerID AS 'Customer ID', FULLNAME AS 'Full Name',
                             Phone, Email
                            FROM dbo.Customers WHERE CUSTOMERID = @CUSTOMERID ORDER BY CUSTOMERID";

            int count = 0;
            DataTable table = new DataTable();
            Customer cus = new Customer();
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
                            table.Load(reader);
                        }
                    }


                    foreach (DataRow row in table.Rows)
                    {
                        cus.CustomerId = row.Field<int>("Customer ID");
                        cus.FullName = row.Field<string>("Full Name");
                        cus.PhoneNumber = row.Field<string>("Phone");
                        cus.email = row.Field<string>("Email");
                        count++;
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


            return (table: table, customers: cus);
        }

        /// <summary>
        /// Method implementation of inserting into the customers table.
        /// It does not take in a CustomerId because the creation of the 
        /// table Customer declared it to be Identity.
        /// </summary>
        /// <returns>The column that is converted into a Customer Object.</returns>
        public static void INSERT_INTO_CUSTOMER(string name, string phone, string email)
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

        }

        public static (Customer before, Customer after) UPDATE_CUSTOMER_DETAILS(int id, string? name, string? phone, string? email)
        {
            string query = @"UPDATE CUSTOMERS
                 SET FULLNAME = @FULLNAME,
                     PHONE = @PHONE,
                     EMAIL = @EMAIL
                 WHERE CUSTOMERID = @CUSTOMERID";
            Customer orgCus = new Customer();
            Customer newCus = new Customer();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        orgCus = SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(id).customer;

                        cmd.Parameters.AddWithValue("@CUSTOMERID", id);
                        cmd.Parameters.AddWithValue("@FULLNAME", name != null ? name : orgCus.FullName);
                        cmd.Parameters.AddWithValue("@PHONE", phone != null ? phone : orgCus.PhoneNumber);
                        cmd.Parameters.AddWithValue("@EMAIL", email != null ? email : orgCus.email);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        newCus = SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(id).customer;

                        

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

            return (before: orgCus, after: newCus);
        }

        public static Customer DELETE_CUSTOMER_WHERE_CUSTOMER_ID(int id)
        {
            var condtion = COUNT_REPAIR_REQUESTS_WHERE_CUSTOMER_ID(id);
            if (!condtion.canDelete)
            {
                MessageBox.Show("YOU CANNOT DELETE A CUSTOMER WITH REMAINING REPAIR REQUEST'S", "CLIENT ERROR");
                throw new Exception("DELET ERROR");
            }

            string query = @"DELETE FROM dbo.Customers WHERE CUSTOMERID = @CUSTOMERID";

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CUSTOMERID", id);

                    connection.Open();

                    Customer cus = SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(id).customer;

                    cmd.ExecuteNonQuery();

                    return cus;
                }
            }

        }




        // Repair Request

        public static (DataTable table, List<RepairRequest> request) SELECT_FROM_REPAIRE_REQUEST()
        {
            string query = @"SELECT r.RepairId AS 'Repair ID', r.CustomerID, c.FullName AS 'Customer Name', r.DeviceType AS 'Device Type', 
                        r.ProblemDescription AS 'Problem', r.RepairDate AS 'Repair Date', r.Status, r.ServiceCost AS 'Service Cost'
                        FROM dbo.RepairRequests AS r INNER JOIN dbo.Customers AS c 
                        ON c.CUSTOMERID = r.CUSTOMERID ORDER BY r.REPAIRID";

            int count = 0;
            DataTable returnData = new DataTable();
            List<RepairRequest> repairrequest = new List<RepairRequest>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            returnData.Load(reader);
                        }
                    }

                    foreach (DataRow row in returnData.Rows)
                    {
                        RepairRequest request = new RepairRequest();

                        request.RepairID = row.Field<int>("Repair ID");
                        request.CustomerID = row.Field<int>("CustomerID");
                        request.DeviceType = row.Field<string>("Device Type");
                        request.ProblemDescription = row.Field<string>("Problem");
                        request.RepairDate = row.Field<DateTime>("Repair Date").Date;
                        request.Status = row.Field<string>("Status");
                        request.ServiceCost = row.Field<decimal>("Service Cost");

                        repairrequest.Add(request);
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

            return (table: returnData, request: repairrequest);
        }

        public static (DataTable table, RepairRequest request) SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(int id = 1)
        {
            string query = @"SELECT r.RepairId AS 'Repair ID', r.CustomerID, c.FullName AS 'Customer Name', r.DeviceType AS 'Device Type', 
                        r.ProblemDescription AS 'Problem', r.RepairDate AS 'Repair Date', r.Status, r.ServiceCost AS 'Service Cost'
                        FROM dbo.RepairRequests AS r INNER JOIN dbo.Customers AS c 
                        ON c.CUSTOMERID = r.CUSTOMERID WHERE r.RepairID = @REPAIRID ORDER BY REPAIRID ";

            DataTable returnData = new DataTable();
            RepairRequest request = new RepairRequest();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@REPAIRID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            returnData.Load(reader);
                        }
                    }
                    
                    foreach (DataRow row in returnData.Rows)
                    {
                        request.RepairID = row.Field<int>("Repair ID");
                        request.CustomerID = row.Field<int>("CustomerID");
                        request.DeviceType = row.Field<string>("Device Type");
                        request.ProblemDescription = row.Field<string>("Problem");
                        request.RepairDate = row.Field<DateTime>("Repair Date").Date;
                        request.Status = row.Field<string>("Status");
                        request.ServiceCost = row.Field<decimal>("Service Cost");

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

            return (table: returnData, request: request);
        }


        public static void INSERT_INTO_REPAIR_REQUEST(int customerId, DeviceType type, string desc, DateTime repairDate, Status status, decimal serviceCost)
        {
            string query = @"INSERT INTO dbo.RepairRequests 
                                (CustomerID, DeviceType, ProblemDescription, RepaireDate, Status, ServiceCost)
                                VALUES
                                (@CustemerID, @DeviceType, @ProblemDescription, @RepaireDate, @Status, @ServiceCost)";

            int count = 0;
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        cmd.Parameters.AddWithValue("@DeviceType", type);
                        cmd.Parameters.AddWithValue("@ProblemDescription", desc);
                        cmd.Parameters.AddWithValue("@RepairDate", repairDate);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@ServiceCost", serviceCost);

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
        }

        public static (RepairRequest before, RepairRequest after) UPDATE_REPAIR_REQUEST_DETAILS(int repairID, int? customerID, DeviceType? type, string? desc, DateTime? repairDate, Status? status, decimal? serviceCost)
        {
            string query = @"UPDATE dbo.RepairRequests SET
                            CustomerID = @CusID,
                            DeviceType = @Type,
                            ProblemDescription = @Problem,
                            RepairDate = @Date,
                            Status = @Status,
                            ServiceCost = @Cost
                            WHERE RepairID = @ID";


            RepairRequest OrgRepair = new RepairRequest();
            RepairRequest NewRepair = new RepairRequest();
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        OrgRepair = SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(repairID).request;
                        cmd.Parameters.AddWithValue("@ID", repairID);
                        cmd.Parameters.AddWithValue("@CusID", customerID != null ? customerID : OrgRepair.CustomerID);
                        cmd.Parameters.AddWithValue("@Type", type != null ? type : OrgRepair.DeviceType);
                        cmd.Parameters.AddWithValue("@Problem", desc != null ? desc : OrgRepair.ProblemDescription);
                        cmd.Parameters.AddWithValue("@Date", repairDate != null ? repairDate : OrgRepair.RepairDate);
                        cmd.Parameters.AddWithValue("@Status", status != null ? status : OrgRepair.Status);
                        cmd.Parameters.AddWithValue("@ServiceCost", serviceCost != null ? serviceCost : OrgRepair.ServiceCost);


                        connection.Open();
                        cmd.ExecuteNonQuery();

                        NewRepair = SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(repairID).request;

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

            return (before: OrgRepair, after: NewRepair);
        }

        public static RepairRequest DELETE_REPAIR_REQUEST_WHERE_REPAIR_ID(int id)
        {
            string query = @"DELETE FROM dbo.RepairRequests WHERE RepairID = @ID";

            RepairRequest request = new RepairRequest();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        connection.Open();

                        request = SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(id).request;

                        cmd.ExecuteNonQuery();

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

            return request;

        }

        // Extra

        public static (bool canDelete, int count) COUNT_REPAIR_REQUESTS_WHERE_CUSTOMER_ID(int id)
        {
            string query = @"SELECT COUNT(*) 
                     FROM dbo.RepairRequests 
                     WHERE CUSTOMERID = @CUSTOMERID";

            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CUSTOMERID", id);

                        int c = Convert.ToInt32(cmd.ExecuteScalar());
                        return (canDelete: (c > 0 ? false : true), count: c);
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

            return (canDelete: false, count: -1);
        }

        
    }
}
