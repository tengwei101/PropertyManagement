using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddAppointment : Page
    {
        private List<Attendee> _attendees = new List<Attendee>();


        public AddAppointment()
        {
            this.InitializeComponent();
            DurationTextBlock.Text = TimeSpan.FromHours(DurationSlider.Value).ToString(@"hh\:mm");

        }

        private void DurationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (DurationTextBlock != null)
            {
                DurationTextBlock.Text = TimeSpan.FromHours(DurationSlider.Value).ToString(@"hh\:mm");
            }
        }


        private async void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new appointment with the entered data
            Appointment appointment = new Appointment
            {
                Id = "",
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                StartDate = StartDatePicker.Date.DateTime.ToString("dd-MM-yyyy"),
                StartTime = StartTimePicker.Time.ToString(@"hh\:mm"),
                Duration = TimeSpan.FromHours(DurationSlider.Value).ToString(@"hh\:mm"),
                Location = LocationTextBox.Text,
                Attendees = _attendees,
                Status = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                PropertyId = GlobalData.property.Id,
            };
            // Save the property object to Firebase Database and get the Firebase key
            string firebaseKey = await CreateAppointmentInFirebaseDatabaseAsync(appointment);

            // Check if the property was successfully saved
            if (firebaseKey != null)
            {
                // Set the property Id to the firebaseKey
                appointment.Id = firebaseKey;

                // Update the property in Firebase Database with the new Id
                try
                {
                    await UpdateAppointmentInFirebaseDatabaseAsync(firebaseKey, appointment);
                    DisplayDialog("Success", "New Appointment has been added.");
                    Frame.Navigate(typeof(AppointmentList));
                }
                catch (Exception ex)
                {
                    DisplayDialog("Error", ex.Message);
                }
            }

            // Clear the input fields
            TitleTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            StartDatePicker.Date = DateTime.Today;
            StartTimePicker.Time = TimeSpan.Zero;
            DurationSlider.Value = 0.5;
            LocationTextBox.Text = string.Empty;
            StatusComboBox.SelectedIndex = -1;
            _attendees.Clear();
            AttendeesListView.ItemsSource = null;
        }


        private async Task<string> CreateAppointmentInFirebaseDatabaseAsync(Appointment appointment)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(appointment);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}appointments.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Dictionary<string, string> responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                return responseData["name"];
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
                return null;
            }
        }

        private async Task UpdateAppointmentInFirebaseDatabaseAsync(string firebaseKey, Appointment appointment)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(appointment);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}appointments/{firebaseKey}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

        private async void AddAttendeeButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddAttendee page
            AddAttendee addAttendeePage = new AddAttendee();

            // Show the AddAttendee page as a modal dialog
            ContentDialog dialog = new ContentDialog
            {
                Title = "Add Attendee",
                Content = addAttendeePage,
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel",
                IsPrimaryButtonEnabled = false
            };

            addAttendeePage.AttendeeInfoChanged += (s, a) =>
            {
                dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(addAttendeePage.AttendeeName)
                    && !string.IsNullOrWhiteSpace(addAttendeePage.AttendeeEmail)
                    && !string.IsNullOrWhiteSpace(addAttendeePage.AttendeePhoneNumber)
                    && !string.IsNullOrWhiteSpace(addAttendeePage.AttendeeRole);
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Attendee attendee = new Attendee
                {
                    Name = addAttendeePage.AttendeeName,
                    Email = addAttendeePage.AttendeeEmail,
                    PhoneNumber = addAttendeePage.AttendeePhoneNumber,
                    Role = addAttendeePage.AttendeeRole
                };

                _attendees.Add(attendee);
                AttendeesListView.ItemsSource = null;
                AttendeesListView.ItemsSource = _attendees;
            }
        }

        private async void DisplayDialog(string title, string context)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = context,
                CloseButtonText = "OK",
            };
            ContentDialogResult result = await contentDialog.ShowAsync();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            Attendee attendeeToDelete = deleteButton.DataContext as Attendee;

            if (attendeeToDelete != null)
            {
                _attendees.Remove(attendeeToDelete);
                AttendeesListView.ItemsSource = null;
                AttendeesListView.ItemsSource = _attendees;
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentList));
        }
    }
}
