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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddAttendee : Page
    {
        public string AttendeeName { get; set; }
        public string AttendeeEmail { get; set; }
        public string AttendeePhoneNumber { get; set; }
        public string AttendeeRole { get; set; }
        public event EventHandler AttendeeInfoChanged;

        public AddAttendee()
        {
            this.InitializeComponent();
            NameTextBox.TextChanged += OnAttendeeInfoChanged;
            EmailTextBox.TextChanged += OnAttendeeInfoChanged;
            PhoneNumberTextBox.TextChanged += OnAttendeeInfoChanged;
            RoleComboBox.SelectionChanged += OnAttendeeInfoChanged;
        }

        private void OnAttendeeInfoChanged(object sender, RoutedEventArgs e)
        {
            AttendeeName = NameTextBox.Text;
            AttendeeEmail = EmailTextBox.Text;
            AttendeePhoneNumber = PhoneNumberTextBox.Text;
            if (RoleComboBox.SelectedItem != null)
            {
                AttendeeRole = (RoleComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
            else
            {
                AttendeeRole = string.Empty;
            }
            AttendeeInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRoleComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = RoleComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                AttendeeRole = selectedItem.Content.ToString();
            }
            else
            {
                AttendeeRole = null;
            }
            AttendeeInfoChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
