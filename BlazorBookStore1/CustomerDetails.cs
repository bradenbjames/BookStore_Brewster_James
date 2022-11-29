using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class CustomerDetails
    {
        public int customerID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }

        public CustomerDetails(int customerID, string fName, string lName, string email, string phone, string address)
        {
            this.customerID = customerID;
            this.fName = fName;
            this.lName = lName;
            this.email = email;
            this.phone = phone;
            this.address = address;
        }
    }
}
