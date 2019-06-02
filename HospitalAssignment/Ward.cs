using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public class Ward
    {
        //ward properties
        public string Name { get; set; }

        public int Capacity { get; set; }

        //collection of patients inside ward
        public ObservableCollection<Patient> Patients { get; set; }

        //constructor 1
        public Ward()
        {
            Patients = new ObservableCollection<Patient>();
        }

        //construcor 2
        public Ward(string name, int capacity) : this()
        {
            Name = name;
            Capacity = capacity;
        }

        //constructor 3
        public Ward(string name) : this()
        {
            Name = name;
        }

        //to string method
        public override string ToString()
        {
            return string.Format($"{Name} (Limit: {Capacity})");
        }
    }
}
