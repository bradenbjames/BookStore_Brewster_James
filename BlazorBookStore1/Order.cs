using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class Order
    {
        public int orderID { get; set; }
        public string orderDate { get; set; }
        public float orderVal { get; set; }
        public int customerID { get; set; }

        public Order(int orderID, string orderDate, float orderVal, int customerID)
        {
            this.orderID = orderID;
            this.orderDate = orderDate;
            this.orderVal = orderVal;
            this.customerID = customerID;
        }
    }
}
