using Firebase.Database;
using Firebase.Database.Query;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using System.Net;
using System.Net.Mail;
using System.Globalization;
using System.Text;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentDetails : Page
    {
        private Appointment appointment;
        private FirebaseClient firebaseClient;

        public AppointmentDetails()
        {
            this.InitializeComponent();
            this.appointment = GlobalData.appointment;
            this.firebaseClient = new FirebaseClient(GlobalData.firebaseDatabase);
            PopulateAppointmentDetails();
        }

        private void PopulateAppointmentDetails()
        {
            appointment = GlobalData.appointment;

            TitleTextBlock.Text = appointment.Title;
            DescriptionTextBlock.Text = appointment.Description;
            DateTimeTextBlock.Text = $"DateTime: {appointment.StartDate} at {appointment.StartTime}";
            DurationTextBlock.Text = $"Duration: {appointment.Duration}";
            LocationTextBlock.Text = $"Location: {appointment.Location}";

            // Set the selected ComboBox item based on the appointment's status
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == appointment.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }

            AttendeesListView.ItemsSource = appointment.Attendees;
        }

        private async void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (appointment != null)
            {
                string newStatus = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString();

                // Update the appointment status locally
                appointment.Status = newStatus;

                // Update the appointment status in Firebase
                await firebaseClient
                    .Child("appointments")
                    .Child(appointment.Id)
                    .Child("Status")
                    .PutAsync($"\"{newStatus}\""); // Wrap the value in double quotes
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditAppointment));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (appointment != null)
            {
                // Create the confirmation dialog
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Delete Appointment",
                    Content = "Are you sure you want to delete this appointment?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No"
                };

                // Show the confirmation dialog and get the result
                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

                // If the user clicked "Yes", delete the appointment
                if (result == ContentDialogResult.Primary)
                {
                    // Delete the appointment in Firebase
                    await firebaseClient
                        .Child("appointments")
                        .Child(appointment.Id)
                        .DeleteAsync();

                    // Navigate back to the AppointmentList page
                    Frame.Navigate(typeof(AppointmentList));
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentList));
        }

        private async void AttendeeItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var attendeeStackPanel = sender as StackPanel;
            var attendee = attendeeStackPanel.DataContext as Attendee;

            // Display a MessageDialog asking the user if they want to send a notification email to the attendee
            var messageDialog = new MessageDialog($"Do you want to send a notification email to {attendee.Name} at {attendee.Email}?", "Send Notification Email");
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.Commands.Add(new UICommand("No"));
            var result = await messageDialog.ShowAsync();

            if (result.Label == "Yes")
            {
                // Send email to the attendee
                await SendNotificationEmailAsync(attendee);
            }
        }

        private void AttendeeItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var attendeeStackPanel = sender as StackPanel;
            attendeeStackPanel.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(70, 255, 182, 197)); // You can choose your desired color and opacity
        }

        private void AttendeeItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var attendeeStackPanel = sender as StackPanel;
            attendeeStackPanel.Background = null;
        }

        private async Task SendNotificationEmailAsync(Attendee attendee)
        {
            try
            {
                // Set up the email sender
                var fromAddress = new MailAddress("tengwei101@gmail.com", "RAD Property Management");
                var toAddress = new MailAddress(attendee.Email, attendee.Name);
                const string subject = "RAD Property Appointment Notification";
                string body = $"Hello {attendee.Name},\n\nYou have an appointment:\n\n" +
                    $"Title: {appointment.Title}\n" +
                    $"Description: {appointment.Description}\n" +
                    $"Date: {appointment.StartDate}\n" +
                    $"Time: {appointment.StartTime}\n" +
                    $"Duration: {appointment.Duration}\n" +
                    $"Location: {appointment.Location}\n" +
                    $"Status: {appointment.Status}\n\n" +
                    $"Best Regards";

                // Create the iCalendar event
                StringBuilder icsBuilder = new StringBuilder();
                icsBuilder.AppendLine("BEGIN:VCALENDAR");
                icsBuilder.AppendLine("VERSION:2.0");
                icsBuilder.AppendLine("PRODID:-//RAD Property Management//Appointment Notification");
                icsBuilder.AppendLine("BEGIN:VEVENT");

                DateTime startDateTime = DateTime.ParseExact(appointment.StartDate + " " + appointment.StartTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                TimeSpan duration = TimeSpan.Parse(appointment.Duration);

                icsBuilder.AppendLine($"DTSTART:{startDateTime.ToUniversalTime():yyyyMMddTHHmmssZ}");
                icsBuilder.AppendLine($"DTEND:{(startDateTime + duration).ToUniversalTime():yyyyMMddTHHmmssZ}");
                icsBuilder.AppendLine($"SUMMARY:{appointment.Title}");
                icsBuilder.AppendLine($"DESCRIPTION:{appointment.Description}");
                icsBuilder.AppendLine($"LOCATION:{appointment.Location}");
                icsBuilder.AppendLine("END:VEVENT");
                icsBuilder.AppendLine("END:VCALENDAR");

                // Create a MemoryStream from the iCalendar event
                MemoryStream icsStream = new MemoryStream(Encoding.UTF8.GetBytes(icsBuilder.ToString()));

                // Configure the SMTP client
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, "mmpgyqwtfawxlpvf")
                };

                // Create the email message and send it
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    // Attach the iCalendar event
                    message.Attachments.Add(new System.Net.Mail.Attachment(icsStream, "Property Appointment.ics", "text/calendar"));

                    await smtp.SendMailAsync(message);
                }

                var messageDialog = new MessageDialog($"Notification email sent to {attendee.Name} at {attendee.Email}.");
                await messageDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog($"Failed to send email to attendee: {ex.Message}");
                await messageDialog.ShowAsync();
            }
        }


    }

}
