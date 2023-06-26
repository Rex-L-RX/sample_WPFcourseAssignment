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



namespace View{
    
   
    
    public partial class MainWindow : Window{
        private const string Key = "researcherList";
        private ResearcherController researcherController;
        public MainWindow(){

            //RAPAdapter.LoadAll();
            InitializeComponent();
            researcherController = (ResearcherController)(Application.Current.FindResource(Key) as ObjectDataProvider).ObjectInstance;
            CumulitiveCount.Visibility = System.Windows.Visibility.Hidden;

        }

        private void Window_loaded(object sender, RoutedEventArgs a){
           
        }
        


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs a){
            
            if (a.AddedItems.Count > 0){
                DetialsPanel.DataContext = a.AddedItems[0];
                Photo.DataContext = a.AddedItems[0];
                CumulitiveCount.DataContext = a.AddedItems[0];
                
                listbox_Publication.DataContext=a.AddedItems[0];
            }
            
        }

        private void Listbox_Publication_SelectionChanged(object sender, SelectionChangedEventArgs a){
            if (researcherController == null){
            }
            else{
                PublicationPanel.DataContext = listbox_Publication.SelectedItem;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs a){
            SupervisionName sun = new SupervisionName();
            sun.DataContext = showName.DataContext;
            sun.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs a){

            if (CumulitiveCount.Visibility == System.Windows.Visibility.Hidden)
            {
                CumulitiveCount.Visibility = System.Windows.Visibility.Visible;
            }
            else
                CumulitiveCount.Visibility = System.Windows.Visibility.Hidden;
        }

        private void T1_TextChanged(object sender, TextChangedEventArgs a)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs a){

            Researcher r = listbox_Researcher.SelectedItem as Researcher;

            
            int start = Int32.Parse(T1.Text);
            int end = Int32.Parse(T2.Text);
            var result = from Publication p in r.Publi
                         where (p.Year >= start) && (p.Year <= end)
                         select p;
           
            listbox_Publication.ItemsSource = result;
        }

        private void Button_Click_4(object sender, RoutedEventArgs a){
            List<Publication> lstPub = ((Researcher)listbox_Researcher.SelectedItem).Publi;
            
            listbox_Publication.ItemsSource = null;
            listbox_Publication.ItemsSource = lstPub;
        }





    }
}
