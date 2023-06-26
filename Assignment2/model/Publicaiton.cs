using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum PublicationType
    {
        Conference, Journal, Other
    }
    public class Publication
    {
        public string DOI { get; set; }
        public string PublicationTitle { get; set; }
        public string Authors { get; set; }
        public Int32 Year { get; set; }
        public DateTime AvailableDate { get; set; }
        public PublicationType PublicationType { get; set; }
        public int PubCount { get; set; }
        public string Cite_as { get; set; }


      // the age for caculating three year average 
        public double Age{
            get{
                return Math.Round((((DateTime.Today - AvailableDate).Days) / 365.0), 1);
            }
        }

       
        public override string ToString(){
            return "'"+PublicationTitle + "' in " + AvailableDate.Year;
        }

        public int PublicationAge{
            get{
                return (DateTime.Today - AvailableDate).Days;
            }
        }
    }
}