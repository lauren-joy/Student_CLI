using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib.DTO
{
    public class CsvLine
    {

        [Name("firstName")]
        public string FirstName { get; set; }

        [Name("lastName")]
        public string LastName { get; set; }

        [Name("id")]
        public int Id { get; set; }

        [Name("major")]
        public string Major { get; set; }

        [Name("gpa")]
        public double GPA { get; set; }


        [Name("cc")]
        public int CC { get; set; }


        [Name("cps")]
        public int CPS { get; set; }


        [Name("birthDate")]
        public DateTime BirthDate { get; set; }

        [Name("dateEnrolled")]
        public DateTime DateEnrolled { get; set; }

        [Name("graduationDate")]
        public DateTime GraduationDate { get; set; }

        [Name("status")]
        public string Status { get; set; }

    
    }
}
