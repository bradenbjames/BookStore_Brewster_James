using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class Book
    {
        public string isbnNum { get; set; }
        public string title { get; set; }
        public string pubDate { get; set; }
        public float price { get; set; }
        public float reviews { get; set; }
        public int supplierID { get; set; }

        public Book(string isbnNum, string title, string pubDate, float price, float reviews, int supplierID)
        {
            this.isbnNum = isbnNum;
            this.title = title;
            this.pubDate = pubDate;
            this.price = price;
            this.reviews = reviews;
            this.supplierID = supplierID;
        }
    }
}
