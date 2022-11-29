using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class Supplier
    {
        public int supplierID { get; set; }
        public string name { get; set; }

        public Supplier(int supplierID, string name)
        {
            this.supplierID = supplierID;
            this.name = name;
        }
    }
}
