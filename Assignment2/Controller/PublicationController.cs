using Database;
using Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Controller
{
    //Show the details of publication in a new view.
    public class PublicationCount
    {
        public int Year { get; set; }
        public int Count { get; set; }

        private RAPDBAdapter dbadapter = null;

        //Get the quantity of the publications 
        public override string ToString()
        {

            return "Year: " + Year + "  Quantity:   " + Count;

        }
        public ObservableCollection<Publication> GetPublicationsByRange(int rid, int start, int end)//get publication by year range
        {

            ObservableCollection<Publication> allPublications = new ObservableCollection<Publication>();

            allPublications = dbadapter.GetPublicationsByRID(rid);

            ObservableCollection<Publication> filteredPublications = new ObservableCollection<Publication>();
            var result = from Publication c in allPublications
                         where (c.Year >= start) && (c.Year <= end)
                         select c;

            ObservableCollection<Publication> filterPublications = new ObservableCollection<Publication>(result.ToList());
            return filteredPublications;
        }


        public ObservableCollection<Publication> FilterByRange(int rid, int starty, int endy)
        {

            return dbadapter.FilterPublicationByRange(rid, starty, endy);

        }
    }



}
