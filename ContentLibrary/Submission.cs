using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContentLibrary
{
    public class Submission
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int ProductSerialNr { get; set; }

        public Submission() { }

        public Submission(string fName, string lName, string email, int serialNr)
        {
            FirstName = fName;
            LastName = lName;
            Email = email;
            ProductSerialNr = serialNr;
        }
    }
}
