using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentList : Page
    {

        private readonly FirebaseClient _firebaseClient = new FirebaseClient(GlobalData.firebaseDatabase);
        public ObservableCollection<Appointment> FilteredAppointments { get; set; } = new ObservableCollection<Appointment>();


        public AppointmentList()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string initialStatus = ((ComboBoxItem)StatusFilterComboBox.SelectedItem).Content.ToString();
            await RetrieveAndFilterAppointmentsAsync(initialStatus);
        }


        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddAppointment));
        }



        public async Task RetrieveAndFilterAppointmentsAsync(string selectedStatus = "All")
        {
            var allAppointments = await _firebaseClient
                .Child("appointments")
                .OnceAsync<Appointment>();

            FilteredAppointments.Clear();

            var filteredAppointments = allAppointments
                .Where(a => a.Object.PropertyId == GlobalData.property.Id);

            if (selectedStatus != "All")
            {
                filteredAppointments = filteredAppointments
                    .Where(a => a.Object.Status == selectedStatus);
            }

            var orderedAppointments = filteredAppointments
                .OrderBy(a => DateTime.ParseExact(a.Object.StartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .ThenBy(a => TimeSpan.Parse(a.Object.StartTime))
                .Select(a => a.Object);

            foreach (var appointment in orderedAppointments)
            {
                FilteredAppointments.Add(appointment);
            }

            AppointmentListView.ItemsSource = FilteredAppointments;
        }




        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PropertyDetails));
        }

        private void AppointmentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAppointment = (Appointment)AppointmentListView.SelectedItem;

            if (selectedAppointment != null)
            {
                // Perform the desired action on the selected appointment
                // For example, you can navigate to another page with the appointment details
                // Navigate to the PropertyDetails page with the selected property
                GlobalData.appointment = selectedAppointment;
                Frame.Navigate(typeof(AppointmentDetails));

            }
        }


        private async void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            string selectedItem = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedItem == null) return;

            await RetrieveAndFilterAppointmentsAsync(selectedItem);
        }

    }
}
