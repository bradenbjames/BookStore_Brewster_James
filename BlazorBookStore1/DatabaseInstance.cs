using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BlazorBookStore1
{
    public static class DatabaseInstance
    {
        private static string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\Legen\Source\Repos\BookStore_Brewster_James\BlazorBookStore1\Resources\BookStoreDB.mdf;";

        public static void CreateAccount(string fName, string lName, string email, string password, bool isAdministrator, string address, int phone)
        {
            int adminPrivileges = 0;
            if (isAdministrator)
            {
                adminPrivileges = 1;
            }
            string query = $"INSERT INTO dbo.Customer VALUES ('{fName}', '{lName}')";
            int customerID = 0;
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();

                query = $"SELECT * FROM dbo.Customer WHERE fName='{fName}' AND lName='{lName}'";
                command = new SqlCommand(query, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerID = reader.GetInt32(reader.GetOrdinal("customerID"));
                    }
                }
                query = $"INSERT INTO dbo.Login VALUES ({customerID}, '{email}', '{password}', {adminPrivileges})";
                command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
                query = $"INSERT INTO dbo.CustomerContactDetails VALUES ({customerID}, '{address}', '{email}', {phone})";
                command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
            }
        }

        public static void Login(string email, string password)
        {
            int customerID = -1;
            string query = $"SELECT * FROM dbo.Login WHERE email='{email}' AND password='{password}'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer.customerID = reader.GetInt32(reader.GetOrdinal("customerID"));
                        Customer.email = reader.GetString(reader.GetOrdinal("email"));
                        Customer.password = reader.GetString(reader.GetOrdinal("password"));
                        int isAdministrator = reader.GetInt32(reader.GetOrdinal("isAdministrator"));
                        if (isAdministrator == 1)
                            Customer.isAdministrator = true;
                        else
                            Customer.isAdministrator = false;
                        break;
                    }
                }
                query = $"SELECT * FROM dbo.Customer WHERE customerID={customerID}";
                command = new SqlCommand(query, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer.fName = reader.GetString(reader.GetOrdinal("fName"));
                        Customer.lName = reader.GetString(reader.GetOrdinal("lName"));
                        break;
                    }
                }

                query = $"SELECT * FROM dbo.CustomerContactDetails WHERE customerID={customerID}";
                command = new SqlCommand(query, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer.address = reader.GetString(reader.GetOrdinal("address"));
                        Customer.phone = reader.GetInt32(reader.GetOrdinal("phone"));
                    }
                }
            }
        }

        public static void Logout()
        {
            Customer.customerID = -1;
        }
    }
}
