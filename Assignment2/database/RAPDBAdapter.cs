using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using Model;
using Controller;
using System.Collections.ObjectModel;



namespace Database{
    public class RAPDBAdapter{
        private static bool reportingErrors = false;

        //Connect to the database
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection connection = null;
        public ObservableCollection<Researcher> allReseachers = null;
        public ObservableCollection<Position> allPositions = null;
        public ObservableCollection<Publication> allPublications = null;



        //Convert strings to enums
        public static T ParseEnum<T>(string value){
            return (T)Enum.Parse(typeof(T), value);
        }


        //Connection to the database.
        private static MySqlConnection GetConnection(){
            if (connection == null) { 
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                connection = new MySqlConnection(connectionString);
            }
            return connection;
        }

        //Load the reseracher table.
        public static List<Researcher> LoadAll(){
            List<Researcher> staff = new List<Researcher>();

            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;

            try{
                connection.Open(); //open connection

                MySqlCommand cmd = new MySqlCommand(" SELECT id, given_name, family_name, title, photo, campus, IFNULL(level,'Student'), unit, email, current_start, utas_start from researcher", connection);
                rdr = cmd.ExecuteReader();

                while (rdr.Read()){
                    //get the data from researcher table
                    string lvl = rdr.GetValue(6).ToString();
                    staff.Add(new Researcher{

                        ID = rdr.GetInt32(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        Title = rdr.GetString(3),
                        Photo = rdr.GetString(4),
                        Campus = rdr.GetString(5),
                        Level = lvl,
                        Unit = rdr.GetString(7),
                        Email = rdr.GetString(8),
                        CurrentDate = rdr.GetDateTime(9),
                        StartDate = rdr.GetDateTime(10)

                    });
                }
            }

            catch (MySqlException a){  //report error
                ReportError("loading staff", a);
            }
            finally{
                if (rdr != null){
                    rdr.Close();
                }
                if (connection != null){
                    connection.Close();
                }
            }
            return staff;
        }

        //Using switch case method to identify the level
        public static string GetLevelName(string level) {
            //initialise the variable title
            string title = "";

            switch (level) 
            { 
                case (string)("Student"):
                    title = "Student";
                    break;
                case (string)("A"):
                    title = "PostDoc";
                    break;
                case (string)("B"):
                    title = "Lecturer";
                    break;
                case (string)("C"):
                    title = "Senior Lecturer";
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

        //get data from position table.
        public static List<Position> LoadPosition(int id){
            List<Position> pos = new List<Position>();
            MySqlConnection connection = GetConnection(); //connet to db
            MySqlDataReader rdr = null;

            try{
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select level, Start, End from position where end is not null and id=?id", connection);

                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read()){
                    pos.Add(new Position{

                        Level = rdr.GetString(0),
                        StartDate = rdr.GetDateTime(1),
                        EndDate = rdr.GetDateTime(2),
                        LevelName = GetLevelName(rdr.GetString(0))

                    });
                }
            }
            catch (MySqlException a){//report error
                ReportError("loading postion", a);
            }
            finally{//disconnet
                if (rdr != null){
                    rdr.Close();
                }
                if (connection != null){
                    connection.Close();
                }
            }

            return pos;
        }

        //get data from Publication table.
        public static List<Publication> LoadPublication(int id) { 
            List<Publication> pub = new List<Publication>();
            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;
            try{
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select p.DOI, Title, Authors, year, type, cite_as, available from publication as p, researcher_publication as resp where p.doi=resp.doi and researcher_id=?id order by year desc, Title asc", connection);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read()){
                    pub.Add(new Publication{

                        DOI = rdr.GetString(0),
                        PublicationTitle = rdr.GetString(1),
                        Authors = rdr.GetString(2),
                        Year = rdr.GetInt32(3),
                        PublicationType = ParseEnum<PublicationType>(rdr.GetString(4)),
                        Cite_as = rdr.GetString(5),
                        AvailableDate = rdr.GetDateTime(6),

                    });
                }
            }
            catch (MySqlException a){//report error
                ReportError("loading Publication", a);
            }
            finally{
                if (rdr != null){
                    rdr.Close();
                }
                if (connection != null){
                    connection.Close();
                }
            }

            return pub;
        }

        //get the name for supervisor through their ID
        public static string GetName(int sid){

            string SupervisorName = "null";
            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;
            try{
                connection.Open();// connet to db
                MySqlCommand cmd = new MySqlCommand("Select given_name, family_name from researcher where id=?sid", connection);
                cmd.Parameters.AddWithValue("sid", sid);
                rdr = cmd.ExecuteReader();

                while (rdr.Read()){
                    SupervisorName = rdr.GetString(0) + " " + rdr.GetString(1);
                }
            }
            catch (MySqlException a){
                //reprot error
                ReportError("loading Publication", a);
            }
            finally{
                if (rdr != null){
                    rdr.Close();
                }
                if (connection != null){
                    connection.Close();
                }
            }

            return SupervisorName;
        }

        //get data for supervisors
        public static Staff LoadStaff(int id){
            Staff staff = new Staff();
            List<Researcher> students = new List<Researcher>();

            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;

            try{
                connection.Open();// connection open
                
                MySqlCommand cmd = new MySqlCommand("select te.id, st.id, st.given_name, st.family_name from researcher te, researcher st where te.id = st.supervisor_id and te.id=?id", connection);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()){

                    students.Add(new Researcher { GivenName = rdr.GetString(2), FamilyName = rdr.GetString(3) });

                }
            }
            catch (MySqlException a){//report error
                ReportError("loading work", a);
            }
            finally{
                if (rdr != null){
                    rdr.Close();
                }
                if (connection != null){
                    connection.Close();
                }
            }

            staff.Student = students;

            return staff;

        }


        public static Student LoadStudent(int id){//load student
            Student student = new Student();
            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;
            try{
                connection.Open();
                
                MySqlCommand cmd = new MySqlCommand("SELECT degree, supervisor_id from researcher where researcher.id=?id", connection);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()){

                  student.Degree = rdr.GetString(0);
                  student.SupervisorID = rdr.GetInt32(1);

                }
            }
            catch (MySqlException a){

                ReportError("loading work", a);
            }
            finally{

                if (rdr != null){

                    rdr.Close();
                }
                if (connection != null){

                    connection.Close();
                }
            }

            return student;
        }

        //get data from publication table in order to Cumulate publication information
        public static List<PublicationCount> PublicationCumulative(int id){

            List<PublicationCount> cum = new List<PublicationCount>();

            MySqlConnection connection = GetConnection();
            MySqlDataReader rdr = null;
            try{

                connection.Open();
                MySqlCommand cmd = new MySqlCommand("select r.id,p.year, count(p.doi) "
                                                + "from researcher r, researcher_publication rp, publication p "
                                                + "where r.id = rp.researcher_id and p.doi = rp.doi and r.id=?id "
                                                + "group by r.id, p.year", connection);


                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()){

                    cum.Add(new PublicationCount{

                        Year = rdr.GetInt32(1),
                        Count = rdr.GetInt32(2)
                    });

                }
            }
            catch (MySqlException a){
                ReportError("loading postion", a);
            }
            finally{
                if (rdr != null){

                    rdr.Close();
                }
                if (connection != null){

                    connection.Close();
                }
            }

            return cum;
        }

        private static void ReportError(string msg, Exception a){
            if (reportingErrors){

                MessageBox.Show("An error occurred while " + msg + ". Try again later.\n\nError Details:\n" + a,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Publication> GetPublicationsByRID(int rid)
        {
            ObservableCollection<Publication> filteredPublications = new ObservableCollection<Publication>();
            try
            {
                connection.Open();
           

                string query = "SELECT * FROM `publication` WHERE doi in (SELECT `doi` FROM `researcher_publication` WHERE researcher_id=?rid) order by year ASC;";
                MySqlCommand sqlQuery = new MySqlCommand(query, connection);
                sqlQuery.Parameters.AddWithValue("rid", rid);

                MySqlDataReader mySqlReader = sqlQuery.ExecuteReader();

                while (mySqlReader.Read())
                {
                    Publication p = new Publication();
                    p.DOI = mySqlReader.GetString(0);
                    p.PublicationTitle = mySqlReader.GetString(1);
                    p.Authors = mySqlReader.GetString(2);
                    p.Year = mySqlReader.GetInt32(3);
                    p.PublicationType = ParseEnum<PublicationType>(mySqlReader.GetString(4));
                    p.Cite_as = mySqlReader.GetString(5);
                    p.AvailableDate = mySqlReader.GetDateTime(6);

                    filteredPublications.Add(p);

                }
            }
            catch (MySqlException error)
            {
                Console.WriteLine("loading postition error : " + error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return filteredPublications;

        }
        public ObservableCollection<Publication> FilterPublicationByRange(int rid, int start, int end)
        {
            ObservableCollection<Publication> getPublications = new ObservableCollection<Publication>();
            try
            {
                connection.Open();

                string query = "SELECT * FROM `publication` WHERE doi in (SELECT `doi` FROM `researcher_publication` WHERE researcher_id=?rid) and year between( start, end) order by year ASC;";

                MySqlCommand sqlQuery = new MySqlCommand(query, connection);
                sqlQuery.Parameters.AddWithValue("rid", rid);
                sqlQuery.Parameters.AddWithValue("start", start);
                sqlQuery.Parameters.AddWithValue("end", end);

                MySqlDataReader mySqlReader = sqlQuery.ExecuteReader();

                while (mySqlReader.Read())
                {
                    string d = mySqlReader.GetString(0);
                    string t = mySqlReader.GetString(1);
                    string a = mySqlReader.GetString(2);
                    int y = mySqlReader.GetInt32(3);
                    PublicationType type = ParseEnum<PublicationType>(mySqlReader.GetString(4));
                    string c = mySqlReader.GetString(5);
                    DateTime avaiablity = mySqlReader.GetDateTime(6);

                    Publication p = new Publication() { DOI = d, PublicationTitle = t, Authors = a, Year = y, Cite_as = c, AvailableDate = avaiablity };

                    getPublications.Add(p);

                }
            }
            catch (MySqlException error)
            {
                Console.WriteLine("loading postition error : " + error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return getPublications;
        }

    }

}
