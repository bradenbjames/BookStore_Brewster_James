using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public static class Customer
    {
        public static int customerID { get; set; } = -1;
        public static string fName { get; set; }
        public static string lName { get; set; }
        public static string password { get; set; }
        public static string email { get; set; }
        public static int phone { get; set; }   
        public static string address { get; set; }
        public static bool isAdministrator { get; set; }
    }
}
