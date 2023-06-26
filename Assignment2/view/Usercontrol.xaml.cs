using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller;
using Model;
using Database;



namespace View
{
    /// <summary>
    /// Interaction logic for Usercontrol.xaml
    /// </summary>
    public partial class Usercontrol : UserControl
    {
        private const string Key = "researcherList";
        private ResearcherController researcherController;

        public Usercontrol()
        {
            InitializeComponent();
            researcherController = (ResearcherController)(Application.Current.FindResource(Key) as ObjectDataProvider).ObjectInstance;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs a)
        {
            var lstEmpLvl = Enum.GetValues(typeof(EmploymentLevelEnum)).Cast<EmploymentLevelEnum>();
            Combo1.ItemsSource = lstEmpLvl;
            Combo1.SelectedIndex = 0;

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs a) //Searching by key word achieved here through LINQ
        {

            if (researcherController == null)
            {

            }
            else
            {
                string name = SearchKey.Text;
                string level = Combo1.SelectedItem.ToString();
                researcherController.Filter(name, level);
            }
        }

        private void Combo1_SelectionChanged(object sender, SelectionChangedEventArgs a)
        {
            if (researcherController == null)
            {

            }
            else
            {
                string name = SearchKey.Text;
                string level = null;
                if (null == Combo1.SelectedItem)
                {
                    level = "All";
                }
                else
                {
                    level = Combo1.SelectedItem.ToString();
                }
                researcherController.Filter(name, level);
            }
        }
    }
}
