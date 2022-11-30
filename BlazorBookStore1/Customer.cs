using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class Customer
    {
        public int customerID { get; set; } = -1;
        public string fName { get; set; }
        public string lName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }   
        public string address { get; set; }
        public bool isAdministrator { get; set; } = false;
    }
}
