using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    //enum for blood type
    public enum BloodType { A, B, AB, O }

    public class Patient
    {
        //properties of patients
        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public int Age { get; set; }

        public BloodType Blood { get; set; }

        public string BloodImage
        {
            get
            {
                return string.Format("/images/{0}.png", Blood);
            }

        }

        //constructor 1
        public Patient(string name, DateTime dob, BloodType blood)
        {
            Name = name;
            DOB = dob;
            Blood = blood;
            Age = GetAge(dob);
        }

        //constructor 2
        public Patient(string name, BloodType blood)
        {
            Name = name;
            Blood = blood;
        }

        //constructor 3 - empty
        public Patient()
        {
        }

        //method for calculating age
        public int GetAge(DateTime dob)
        {
            TimeSpan Span;
            Span = DateTime.Now.Subtract(dob);
            Age = Span.Days / 365;
            return Age;
        }

        //to string method
        public override string ToString()
        {
            return string.Format($"{Name} ({Age} years) Type: {Blood}");
        }

    }
}
