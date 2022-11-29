using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class Author
    {
        public int authorID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string gender { get; set; }
        public string DOB { get; set; }

        public Author(int authorID, string fName, string lName, string gender, string DOB)
        {
            this.authorID = authorID;
            this.fName = fName;
            this.lName = lName;
            this.gender = gender;
            this.DOB = DOB;
        }
    }
}
