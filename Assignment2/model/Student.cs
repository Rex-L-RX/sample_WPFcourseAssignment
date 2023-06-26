using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Model{
    //The student class which inherit Researcher
    public class Student:Researcher{
        public string Degree { get; set; }
        public int SupervisorID { get; set; }
        public string SupervisorName { get { return RAPDBAdapter.GetName(SupervisorID); } }
    }


}
