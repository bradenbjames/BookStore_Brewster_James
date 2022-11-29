using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlazorBookStore1.Pages;

namespace BlazorBookStore1
{
    public static class DatabaseInstance
    {
        private static string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\Legen\Source\Repos\BookStore_Brewster_James\BlazorBookStore1\Resources\BookStoreDB.mdf;";

        public static void CreateAccount(string fName, string lName, string email, string password, bool isAdministrator, string address, long phone)
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
                conn.Open();
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

        public static List<Customers> viewCustomers()
        {
            List<Customers> customers = new List<Customers>();
            string query = $"SELECT * FROM dbo.Customer JOIN dbo.CustomerContactDetails ON dbo.Customer.customerID = dbo.CustomerContactDetails.customerID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int customerID = reader.GetInt32(reader.GetOrdinal("customerID"));
                        string fName = reader.GetString(reader.GetOrdinal("fName"));
                        string lName = reader.GetString(reader.GetOrdinal("lName"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        int phone = reader.GetInt32(reader.GetOrdinal("phone"));
                        string address = reader.GetString(reader.GetOrdinal("address"));

                        Customers newCustomer = new Customers(customerID, fName, lName, email, phone, address);
                        customers.Add(newCustomer);
                    }
                }
            }
            return customers;
        }

        public static List<Order> viewOrders()
        {
            List<Order> orders = new List<Order>();
            string query = $"SELECT * FROM dbo.Orders";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int orderID = reader.GetInt32(reader.GetOrdinal("orderID"));
                        string orderDate = reader.GetString(reader.GetOrdinal("orderDate"));
                        float orderVal = reader.GetFloat(reader.GetOrdinal("orderVal"));
                        int customerID = reader.GetInt32(reader.GetOrdinal("customerID"));

                        Order newOrder = new Order(orderID, orderDate, orderVal, customerID);
                        orders.Add(newOrder);
                    }
                }
            }
            return orders;
        }
        public static List<Book> viewBooks()
        {
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM dbo.Books";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string isbnNum = reader.GetString(reader.GetOrdinal("isbnNum"));
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string pubDate = reader.GetString(reader.GetOrdinal("pubDate"));
                        float price = reader.GetFloat(reader.GetOrdinal("price"));
                        float reviews = reader.GetFloat(reader.GetOrdinal("reviews"));
                        int supplierID = reader.GetInt32(reader.GetOrdinal("supplierID"));

                        Book newBook = new Book(isbnNum, title, pubDate, price, reviews, supplierID);
                        books.Add(newBook);
                    }
                }
            }
            return books;
        }

        public static List<Author> viewAuthors()
        {
            List<Author> authors = new List<Author>();
            string query = $"SELECT * FROM dbo.Author";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int authorID = reader.GetInt32(reader.GetOrdinal("authorID"));
                        string fName = reader.GetString(reader.GetOrdinal("fName"));
                        string lName = reader.GetString(reader.GetOrdinal("lName"));
                        string gender = reader.GetString(reader.GetOrdinal("gender"));
                        string DOB = reader.GetString(reader.GetOrdinal("DOB"));

                        Author newAuthor = new Author(authorID, fName, lName, gender, DOB);
                        authors.Add(newAuthor);
                    }
                }
            }
            return authors;
        }


    }
}
