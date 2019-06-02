using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace HospitalAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //collection of wards
        public ObservableCollection<Ward> wardList = new ObservableCollection<Ward>();

        //properties needed for add patient button loops
        DateTime dob;
        BloodType blood;

        public MainWindow()
        {
            InitializeComponent();
        }

        //populate ward list box on load
        #region window loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////create wards
            //Ward w1 = new Ward("Maternity", 7);
            //Ward w2 = new Ward("Emergency", 10);
            //Ward w3 = new Ward("Cardiology", 4);

            ////Create patients
            //Patient p1 = new Patient("Kelly Conlon", Convert.ToDateTime("06/06/1985"), BloodType.AB);
            //Patient p2 = new Patient("Jane Smith", Convert.ToDateTime("16/03/1994"), BloodType.O);
            //Patient p3 = new Patient("Tom Jones", Convert.ToDateTime("22/08/1991"), BloodType.A);
            //Patient p4 = new Patient("Phil Pearsons", Convert.ToDateTime("10/11/2001"), BloodType.A);
            //Patient p5 = new Patient("Susan Beirne", Convert.ToDateTime("10/05/1974"), BloodType.B);
            //Patient p6 = new Patient("Jarome McDermott", Convert.ToDateTime("03/03/1960"), BloodType.B);

            ////Add patients to wards
            //w1.Patients.Add(p1);
            //w1.Patients.Add(p2);
            //w2.Patients.Add(p3);
            //w2.Patients.Add(p4);
            //w3.Patients.Add(p5);
            //w3.Patients.Add(p6);

            ////Add wards to collection
            //wardList.Add(w1);
            //wardList.Add(w2);
            //wardList.Add(w3);

            //connect to a file
            using (StreamReader sr = new StreamReader(@"c:\temp\wards.json"))
            {
                //read text
                string json = sr.ReadToEnd();

                //convert from json to objects
                wardList = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);

                //refresh display
                lbxWards.ItemsSource = wardList;
                tblkCount.Text = Convert.ToString($" ({ wardList.Count})");
            }

            ////Display wards
            //lbxWards.ItemsSource = wardList;

            ////display ward count
            //tblkCount.Text = Convert.ToString($" ({ wardList.Count})");
        }
        #endregion

        //display patients from ward when that ward is selected
        private void LbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //determine what ward was selected
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            if (selectedWard != null)
            {
                //display patients from that ward
                lbxPatients.ItemsSource = selectedWard.Patients;
            }
        }

        //display blood image when patient is selected
        private void LbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //determine what patient was selected
            Patient selectedPatient = lbxPatients.SelectedItem as Patient;

            if (selectedPatient != null)
            {
                imgBlood.Source = new BitmapImage(new Uri(selectedPatient.BloodImage, UriKind.Relative));
            }
        }

        //method showing what number slider is positioned on
        private void SldrCapacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tblkCapacityCount.Text = string.Format("{0:F0}", sldrCapacity.Value);
        }

        //add ward button
        private void BtnWard_Click(object sender, RoutedEventArgs e)
        {
            //read data from screen
            string name = tbxWardName.Text;
            int capacity = Convert.ToInt32(sldrCapacity.Value);

            //create ward object
            Ward newWard = new Ward(name, capacity);

            //add to collection of wards
            wardList.Add(newWard);

            //update ward count
            tblkCount.Text = Convert.ToString($" ({ wardList.Count})");
        }

        //add patient button
        private void BtnPatient_Click(object sender, RoutedEventArgs e)
        {

            //read data from screen
            string name = tbxPatientName.Text;

            //check dob is datetime
            try
            {
                dob = Convert.ToDateTime(tbxDOB.Text);
            }
            //if not show error and return - stops executing rest of code
            catch
            {
                DOBErrorWindow DOBNotValid = new DOBErrorWindow();
                //displays new window
                DOBNotValid.ShowDialog();
                return;
            }

            //checking which bloodtype is picked
            if (rbtnA.IsChecked == true)
            {
                blood = BloodType.A;
            }
            else if (rbtnB.IsChecked == true)
            {
                blood = BloodType.B;
            }
            else if (rbtnAB.IsChecked == true)
            {
                blood = BloodType.AB;
            }
            else if (rbtnO.IsChecked == true)
            {
                blood = BloodType.O;
            }
            //error message if no blood type is chosen
            else
            {
                BloodErrorWindow bloodTypeError = new BloodErrorWindow();
                bloodTypeError.ShowDialog();
                return;
            }

            Ward selectedWard = lbxWards.SelectedItem as Ward;

            //create patient object
            Patient newPatient = new Patient(name, dob, blood);

            //Add patient to ward if there is space
            if (selectedWard.Patients.Count < selectedWard.Capacity)
            {
                selectedWard.Patients.Add(newPatient);
            }
            //else return error message
            else
            {
                WardErrorWindow capacityReached = new WardErrorWindow();
                //displays new window
                capacityReached.ShowDialog();
            }
        }

        //save objects to JSON
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //convert objects into JSON formatted string
            string json = JsonConvert.SerializeObject(wardList, Formatting.Indented);

            //write that to file
            using (StreamWriter sw = new StreamWriter(@"c:\temp\wards.json"))
            {
                sw.Write(json);
            }
        }

        //load json file
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            //connect to a file
            using (StreamReader sr = new StreamReader(@"c:\temp\wards.json"))
            {
                //read text
                string json = sr.ReadToEnd();

                //convert from json to objects
                wardList = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);

                //refresh display
                lbxWards.ItemsSource = wardList;
                tblkCount.Text = Convert.ToString($" ({ wardList.Count})");
            }
        }
    }
}
