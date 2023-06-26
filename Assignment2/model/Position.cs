using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model{

    // Using enum to define employmentlevel
    public enum EmploymentLevelEnum{
        All,
        Student,
        A,
        B,
        C,
        D,
        E
    }



    public class Position{

        public int ID { get; set; }
        public string Level {get;set;}
        public DateTime StartDate {get; set;}
        public DateTime EndDate{get;set;}
        public string LevelName { get; set; }

        public override string ToString(){

            return StartDate.ToShortDateString() + "\t" + EndDate.ToShortDateString() + "\t" + LevelName;
        }
    }

   



}
