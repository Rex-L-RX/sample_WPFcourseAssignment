using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model{
    //The staff class which inherit Researcher
    public class Staff : Researcher{

        public DateTime AvailableDate { get; set; }
        public List<Researcher> Student { get; set; }
       
        //Get the number supervision 
        public int StudentNum{

            get{
                int num = 0;
                num = Student == null ? 0 : Student.Count();
                return num;
            }
        }
    }


}
