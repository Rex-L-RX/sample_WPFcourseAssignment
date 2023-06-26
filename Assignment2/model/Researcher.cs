using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class Researcher
    {
        public int ID { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Campus { get; set; }
        public string Level { get; set; }
        public string Unit { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<Publication> Publi { get; set; }
        public List<Position> Position { get; set; }
        public Student Student { get; set; }
        public Staff Staff { get; set; }

        public List<PublicationCount> Pulist { get; set; }

        //Using swith to get currentJobTitle from level
        public string GetCurrentJobTitle{
            get{
                string title = "";

                switch (Level){

                    case (string)("Student"):
                        title = "Student";
                        break;
                    case (string)("A"):
                        title = "PostDoctor";
                        break;
                    case (string)("B"):
                        title = "Lecturer";
                        break;
                    case (string)("C"):
                        title = "Senior";
                        break;
                    case (string)("D"):
                        title = "Associate Professor";
                        break;
                    case (string)("E"):
                        title = "Professor";
                        break;
                }
                return title;
            }
        }

        //Get publication count 
        public int PublicationCount{
            get { return Publi == null ? 0 : Publi.Count(); }
        }

        //Get Tenure
        public double Tenure{

            get { return ((double)((DateTime.Today - StartDate).Days)) / 365; }
        }

        //Combined GiveName and FamilyName
        public string Name{
            get{
                return this.GivenName + " " + FamilyName;
            }
        }
          
        public override string ToString(){
            return GivenName + ", " + FamilyName + " (" + Title + ")";
        }

        //the arithmetic for caculating threeYearAverage
        public double ThreeYearAverage{

            get{

                double count = 0.0;
                foreach (Publication a in Publi){

                    if ((a.Age < 4) && (a.Age >= 1)){
                        count = count + 1.0;
                    }
                }
                return Math.Round((double)(count / 3), 1);
            }
        }

        //Using switch-case function to caculate performance by threeYearAverage for each level
        public string Performance{

            get{

                string performance = "";
                switch (Level){

                    case "Student":
                        performance = "None";
                        break;
                    case "A":
                        performance = ((ThreeYearAverage) / 0.5 * 100).ToString("f") + "%";
                        break;
                    case "B":
                        performance = ((ThreeYearAverage) / 1.0 * 100).ToString("f") + "%";
                        break;
                    case "C":
                        performance = ((ThreeYearAverage) / 2.0 * 100).ToString("f") + "%";
                        break;
                    case "D":
                        performance = ((ThreeYearAverage) / 3.2 * 100).ToString("f") + "%";
                        break;
                    case "E":
                        performance = ((ThreeYearAverage) / 4.0 * 100).ToString("f") + "%";
                        break;
                }
                return performance;
            }

        }
    }
}