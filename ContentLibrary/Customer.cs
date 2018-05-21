using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContentLibrary
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public Customer() { }

        public Customer(string fName, string lName, string email, int age)
        {
            FirstName = fName;
            LastName = lName;
            Email = email;
            Age = age;
        }
    }
}
