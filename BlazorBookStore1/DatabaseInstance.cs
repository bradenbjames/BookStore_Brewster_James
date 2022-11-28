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

        public static void createAccount(string fName, string lName, string email, string password, bool isAdministrator)
        {
            string query = $"INSERT INTO dbo.Customer VALUES ('{fName}', '{lName}')";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
