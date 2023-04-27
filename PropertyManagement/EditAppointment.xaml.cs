using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class EditAppointment : Page
    {
        private Appointment appointment;
        private List<Attendee> attendees;
        private FirebaseClient firebaseClient;
        public EditAppointment()
        {
            this.InitializeComponent();
            this.firebaseClient = new FirebaseClient(GlobalData.firebaseDatabase);
        }

        private void DurationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (DurationTextBlock != null)
            {
                DurationTextBlock.Text = TimeSpan.FromHours(DurationSlider.Value).ToString(@"hh\:mm");
            }
        }

        private async void PopulateAppointmentDetails()
        {
            appointment = GlobalData.appointment;

            TitleTextBox.Text = appointment.Title;
            DescriptionTextBox.Text = appointment.Description;
            LocationTextBox.Text = appointment.Location;

            if (DateTime.TryParse(appointment.StartDate, out DateTime startDate))
            {
                StartDatePicker.Date = startDate;
            }
            if(TimeSpan.TryParse(appointment.StartTime, out TimeSpan startTime))
            {
                StartTimePicker.Time = startTime;
            }

            // Ensure that appointment.Duration is in the format "hh:mm"
            if (TimeSpan.TryParse(appointment.Duration, out TimeSpan durationTimeSpan))
            {
                double duration = durationTimeSpan.TotalHours;
                DurationSlider.Value = duration;
            }
            else
            {
                // You can remove this message box once you confirm that the format is correct.
                var messageDialog = new MessageDialog("Invalid appointment duration format. Please make sure it is in the format 'hh:mm'.");
                await messageDialog.ShowAsync();
            }



            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == appointment.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }

            attendees = appointment.Attendees;
            AttendeesListView.ItemsSource = attendees;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PopulateAppointmentDetails();
        }

        private async void AttendeeItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var border = sender as Border;
            var attendee = border.DataContext as Attendee;

            var editAttendeePage = new PropertyManagement.EditAttendee(); // Make sure the namespace is correct
            editAttendeePage.LoadAttendee(attendee);

            var contentDialog = new ContentDialog
            {
                Title = "Edit Attendee",
                Content = editAttendeePage,
                PrimaryButtonText = "Update",
                CloseButtonText = "Cancel"
            };

            editAttendeePage.AttendeeInfoChanged += (s, a) => { contentDialog.IsPrimaryButtonEnabled = editAttendeePage.IsAttendeeInfoValid(); };

            var result = await contentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Update the attendee in the list
                var index = attendees.IndexOf(attendee);
                attendees[index] = editAttendeePage.Attendee;
                AttendeesListView.ItemsSource = null;
                AttendeesListView.ItemsSource = attendees;

            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteButton = sender as Button;
            var attendeeToDelete = deleteButton.DataContext as Attendee;

            // Confirm the deletion
            var messageDialog = new MessageDialog("Are you sure you want to delete this attendee?", "Delete Attendee");
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.Commands.Add(new UICommand("No"));
            var result = await messageDialog.ShowAsync();

            if (result.Label == "Yes")
            {
                // Remove the attendee from the local attendees list
                attendees.Remove(attendeeToDelete);

                // Remove the attendee from the appointment object
                appointment.Attendees.Remove(attendeeToDelete);

                // Update the appointment attendees in Firebase
                await firebaseClient
                    .Child("appointments")
                    .Child(appointment.Id)
                    .Child("Attendees")
                    .PutAsync(attendees);

                // Refresh the attendees ListView
                AttendeesListView.ItemsSource = null;
                AttendeesListView.ItemsSource = attendees;
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

                attendees.Add(attendee);
                AttendeesListView.ItemsSource = null;
                AttendeesListView.ItemsSource = attendees;
            }
        }

        private async void UpdateAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            appointment.Title = TitleTextBox.Text;
            appointment.Description = DescriptionTextBox.Text;
            appointment.StartDate = StartDatePicker.Date.DateTime.ToString("dd-MM-yyyy");
            appointment.StartTime = StartTimePicker.Time.ToString(@"hh\:mm");
            appointment.Duration = TimeSpan.FromHours(DurationSlider.Value).ToString(@"hh\:mm");
            appointment.Location = LocationTextBox.Text;
            appointment.Attendees = attendees;
            appointment.Status = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            try
            {
                // Update the appointment in Firebase
                await firebaseClient
                    .Child("appointments")
                    .Child(appointment.Id)
                    .PutAsync(appointment);

                // Display a message to inform the user that the appointment has been updated
                DisplayDialog("Success", "Appointment has been updated.");

                GlobalData.appointment = appointment;

                // Navigate back to the appointment list
                Frame.Navigate(typeof(AppointmentDetails));
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the update operation
                var messageDialog = new MessageDialog("An error occurred while updating the appointment: " + ex.Message);
                await messageDialog.ShowAsync();
            }


        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentDetails));
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
    }
}
