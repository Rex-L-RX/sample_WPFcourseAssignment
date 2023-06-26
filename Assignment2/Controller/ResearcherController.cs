using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Model;
using Database;

namespace Controller{

    class ResearcherController{
        private List<Researcher> researcherList;
        public List<Researcher> Workers { get { return researcherList; } set { } }
        private ObservableCollection<Researcher> ResearcherVisibility;
        public ObservableCollection<Researcher> VisibleResearcher { get { return ResearcherVisibility; } set { } }
      
        
        public ResearcherController(){
            
            //Get resercher details from database.
            researcherList = RAPDBAdapter.LoadAll();  
            ResearcherVisibility = new ObservableCollection<Researcher>(researcherList); 
            
            //foreach the research list
            foreach (Researcher a in researcherList){

                a.Position = RAPDBAdapter.LoadPosition(a.ID);
                a.Pulist = RAPDBAdapter.PublicationCumulative(a.ID);
                a.Publi = RAPDBAdapter.LoadPublication(a.ID);
                
                //Load students
                if (a.Level == "Student"){

                    a.Student = RAPDBAdapter.LoadStudent(a.ID);

                }
                //Load staffs
                else {

                    a.Staff = RAPDBAdapter.LoadStaff(a.ID);
                                
                }
            }
        }
        //Filter
        public void Filter(string name, string level){

            // if the level has not been selected
            SearchByName(name);
            if (level == "All"){

            }
            // The level has been selected
            else{
                SearchByLevel(level);
            }
        }
                
        public ObservableCollection<Researcher> GetViewableList(){
            return VisibleResearcher;
        }

        //Filter the researchers by level
        public void SearchByLevel(string level)
        {

            List<Researcher> view = ResearcherVisibility.ToList();
            foreach (Researcher a in view)
            {

                if (level == a.Level)
                {

                }

                else
                {

                    ResearcherVisibility.Remove(a);
                }
            }
        }

        //Filter the researchers by name
        public void SearchByName(string name){

            var SearchtName = from Researcher a in researcherList
                         where (a.Name.ToLower()).Contains(name.ToLower())
                         select a;
            ResearcherVisibility.Clear(); //hide researcher
            SearchtName.ToList().ForEach(ResearcherVisibility.Add);
        }

      


    }
}
