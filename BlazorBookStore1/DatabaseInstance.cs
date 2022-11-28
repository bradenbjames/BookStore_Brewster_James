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

        public static void CreateAccount(string fName, string lName, string email, string password, bool isAdministrator)
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
            }
        }

        public static void Login(string email, string password)
        {
            string query = $"SELECT * FROM dbo.Login WHERE email='{email}' AND password='{password}'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                }
            }
        }
    }
}
