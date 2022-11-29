using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlazorBookStore1.Pages;

namespace BlazorBookStore1
{
    /// <summary>
    /// TODO: Make queries for searching by book title, etc. Make query to add books to cart. Make query to change order. Make query to place order.
    /// 
    /// </summary>
    public static class DatabaseInstance
    {
        private static string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\brade\Source\Repos\BookStore_Brewster_James\BlazorBookStore1\Resources\BookStoreDB.mdf;Connection Lifetime=120;";

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

        public static void UpdateAccount(int customerID, string fName, string lName, string address, string email, string phone, string password)
        {
            string query = $"UPDATE dbo.Customer SET fName='{fName}', lName='{lName}' WHERE ";
        }

        public static void Logout()
        {
            Customer.customerID = -1;
        }

        public static List<CustomerDetails> viewCustomers()
        {
            List<CustomerDetails> customers = new List<CustomerDetails>();
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
                        string phone = reader.GetString(reader.GetOrdinal("phone"));
                        string address = reader.GetString(reader.GetOrdinal("address"));

                        CustomerDetails newCustomer = new CustomerDetails(customerID, fName, lName, email, phone, address);
                        customers.Add(newCustomer);
                    }
                }
            }
            return customers;
        }

        public static CustomerDetails getCustomer(int customerID)
        {
            CustomerDetails customer = null;
            string query = $"SELECT * FROM dbo.Customer JOIN dbo.CustomerContactDetails ON dbo.Customer.customerID = dbo.CustomerContactDetails.customerID WHERE customerID={customerID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fName = reader.GetString(reader.GetOrdinal("fName"));
                        string lName = reader.GetString(reader.GetOrdinal("lName"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string phone = reader.GetString(reader.GetOrdinal("phone"));
                        string address = reader.GetString(reader.GetOrdinal("address"));

                        customer = new CustomerDetails(customerID, fName, lName, email, phone, address);
                    }
                }
            }
            return customer;
        }

        public static void editCustomer(int customerID, string fName, string lName, string email, long phone, string address)
        {
            string query = $"UPDATE dbo.Customer SET fName='{fName}', lName='{lName}' WHERE customerID={customerID}";
            string query2 = $"UPDATE dbo.CustomerContactDetails SET email='{email}', phone={phone}, address='{address}' WHERE customerID={customerID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                SqlCommand command2 = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
            }
        }

        public static void deleteCustomer(int customerID)
        {
            string query = $"DELETE FROM dbo.Customer WHERE customerID={customerID}";
            string query2 = $"DELETE FROM dbo.CustomerContactDetails WHERE customerID={customerID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                SqlCommand command2 = new SqlCommand(query2, conn);
                conn.Open();
                command2.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
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

        public static Order getOrder(int orderID)
        {
            Order order = null;
            string query = $"SELECT * FROM dbo.Orders WHERE orderID={orderID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string orderDate = reader.GetString(reader.GetOrdinal("orderDate"));
                        float orderVal = reader.GetFloat(reader.GetOrdinal("orderVal"));
                        int customerID = reader.GetInt32(reader.GetOrdinal("customerID"));

                        order = new Order(orderID, orderDate, orderVal, customerID);
                    }
                }
            }
            return order;
        }

        public static void editOrder(int orderID, string orderDate, float orderVal, int customerID)
        {
            string query = $"UPDATE dbo.Orders SET orderDate='{orderDate}', orderVal={orderVal}, customerID={customerID} WHERE orderID={orderID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void deleteOrder(int orderID)
        {
            string query = $"DELETE FROM dbo.Orders WHERE orderID={orderID}";
            string query2 = $"DELETE FROM dbo.Order_Item WHERE orderID={orderID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                SqlCommand command2 = new SqlCommand(query2, conn);
                conn.Open();
                command2.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
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
        public static List<Book> searchBooks(string searchTerm)
        {
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM dbo.Books WHERE isbnNum like '%{searchTerm}%' or title like '%{searchTerm}%'";
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

        public static List<Book> searchBooksByTitle(string searchTerm)
        {
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM dbo.Books WHERE isbnNum like '%{searchTerm}%' or title like '%{searchTerm}%'";
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

        public static Book getBook(string isbnNum)
        {
            Book book = null;
            string query = $"SELECT * FROM dbo.Books WHERE isbnNum='{isbnNum}'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string pubDate = reader.GetString(reader.GetOrdinal("pubDate"));
                        float price = reader.GetFloat(reader.GetOrdinal("price"));
                        float reviews = reader.GetFloat(reader.GetOrdinal("reviews"));
                        int supplierID = reader.GetInt32(reader.GetOrdinal("supplierID"));

                        book = new Book(isbnNum, title, pubDate, price, reviews, supplierID);
                    }
                }
            }
            return book;
        }

        public static void editBook(string isbnNum, string title, string pubDate, float price, float reviews, int supplierID)
        {
            string query = $"UPDATE dbo.Books SET isbnNum='{isbnNum}', title='{title}', pubDate='{pubDate}', price={price}, reviews={reviews}, supplierID={supplierID} WHERE isbnNum='{isbnNum}'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void deleteBook(string isbnNum)
        {
            string query = $"DELETE FROM dbo.Books WHERE isbnNum='{isbnNum}'";
            string query2 = $"DELETE FROM dbo.BookCategories WHERE isbnNum='{isbnNum}'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                SqlCommand command2 = new SqlCommand(query2, conn);
                command2.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
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
        public static List<Supplier> viewSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string query = $"SELECT * FROM dbo.Supplier";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int supplierID = reader.GetInt32(reader.GetOrdinal("supplierID"));
                        string name = reader.GetString(reader.GetOrdinal("name"));

                        Supplier newSupplier = new Supplier(supplierID, name);
                        suppliers.Add(newSupplier);
                    }
                }
            }
            return suppliers;
        }

        public static AuthorDetails getAuthor(int authorID)
        {
            //yolo
            AuthorDetails author = null;
            string query = $"SELECT * FROM dbo.Author JOIN dbo.AuthorContactDetails ON dbo.Author.authorID=dbo.AuthorContactDetails.authorID WHERE authorID={authorID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fName = reader.GetString(reader.GetOrdinal("fName"));
                        string lName = reader.GetString(reader.GetOrdinal("lName"));
                        string gender = reader.GetString(reader.GetOrdinal("gender"));
                        string DOB = reader.GetString(reader.GetOrdinal("DOB"));
                        string address = reader.GetString(reader.GetOrdinal("address"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string phone = reader.GetString(reader.GetOrdinal("phone"));

                        author = new AuthorDetails(authorID, fName, lName, gender, DOB, address, email, phone);
                    }
                }
            }
            return author;
        }

        public static void editAuthor()
        {
            //finish later and edit Author/AuthorContactDetails
        }

        public static Supplier getSupplier(int supplierID)
        {
            Supplier supplier = null;
            string query = $"SELECT * FROM dbo.Supplier WHERE supplierID={supplierID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("name"));

                        supplier = new Supplier(supplierID, name);
                    }
                }
            }
            return supplier;
        }

        public static void editSupplier(int supplierID, string name)
        {
            string query = $"UPDATE dbo.Supplier SET name='{name}' WHERE supplierID={supplierID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void deleteSupplier(int supplierID)
        {
            string query = $"DELETE FROM dbo.Supplier WHERE supplierID={supplierID}";
            string query2 = $"DELETE FROM dbo.SupplierRep WHERE supplierID={supplierID}";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                SqlCommand command2 = new SqlCommand(query2, conn);
                conn.Open();
                command2.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
        }
    }
}
