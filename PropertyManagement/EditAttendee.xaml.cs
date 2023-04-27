using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
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
    public sealed partial class EditAttendee : Page
    {
        public Attendee Attendee { get; set; }
        public event EventHandler AttendeeInfoChanged;

        public EditAttendee()
        {
            this.InitializeComponent();
        }

        public void LoadAttendee(Attendee attendee)
        {
            Attendee = attendee;
            NameTextBox.Text = attendee.Name;
            EmailTextBox.Text = attendee.Email;
            PhoneNumberTextBox.Text = attendee.PhoneNumber;
            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if (item.Content.ToString() == attendee.Role)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void OnAttendeeInfoChanged(object sender, RoutedEventArgs e)
        {
            Attendee.Name = NameTextBox.Text;
            Attendee.Email = EmailTextBox.Text;
            Attendee.PhoneNumber = PhoneNumberTextBox.Text;
            if (RoleComboBox.SelectedItem != null)
            {
                Attendee.Role = (RoleComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
            else
            {
                Attendee.Role = string.Empty;
            }
            AttendeeInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRoleComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = RoleComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                Attendee.Role = selectedItem.Content.ToString();
            }
            else
            {
                Attendee.Role = null;
            }
            AttendeeInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsAttendeeInfoValid()
        {
            return !string.IsNullOrWhiteSpace(Attendee.Name)
                && !string.IsNullOrWhiteSpace(Attendee.Email)
                && !string.IsNullOrWhiteSpace(Attendee.PhoneNumber)
                && !string.IsNullOrWhiteSpace(Attendee.Role);
        }

    }
}
