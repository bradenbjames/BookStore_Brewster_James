using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBookStore1
{
    public class AuthorDetails
    {
        public AuthorDetails(int authorID, string fName, string lName, string gender, string dOB, string address, string email, string phone)
        {
            AuthorID = authorID;
            FName = fName;
            LName = lName;
            Gender = gender;
            DOB = dOB;
            Address = address;
            Email = email;
            Phone = phone;
        }

        public int AuthorID { get; set; }
        public string FName { get; set;  }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set;  }
        public string Address { get; set;  }
        public string Email { get; set; }
        public string Phone { get; set;  }
    }
}
