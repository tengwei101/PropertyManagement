using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class EditTenant : Page
    {
        private TenantItem selectedTenant;
        public EditTenant()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            selectedTenant = GlobalData.tenant;

            if (selectedTenant != null)
            {
                NameTextBox.Text = selectedTenant.Name;
                EmailTextBox.Text = selectedTenant.Email;
                PhoneNumberTextBox.Text = selectedTenant.PhoneNumber;
                DateOfBirthDatePicker.Date = DateTimeOffset.Parse(selectedTenant.DateOfBirth);
                LeaseStartDatePicker.Date = DateTimeOffset.Parse(selectedTenant.LeaseStartDate);
                LeaseEndDatePicker.Date = DateTimeOffset.Parse(selectedTenant.LeaseEndDate);
                RentAmountTextBox.Text = selectedTenant.RentAmount.ToString();
                SetRentPaymentFrequency(selectedTenant.RentPaymentFrequency);
                IsActiveTenantCheckBox.IsChecked = selectedTenant.IsActiveTenant;
            }
        }

        private void SetRentPaymentFrequency(string rentPaymentFrequency)
        {
            foreach (ComboBoxItem item in RentPaymentFrequencyComboBox.Items)
            {
                if (item.Content.ToString() == rentPaymentFrequency)
                {
                    RentPaymentFrequencyComboBox.SelectedItem = item;
                    break;
                }
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

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the selectedTenant object with the new values from the controls
            selectedTenant.Name = NameTextBox.Text;
            selectedTenant.Email = EmailTextBox.Text;
            selectedTenant.PhoneNumber = PhoneNumberTextBox.Text;
            selectedTenant.DateOfBirth = DateOfBirthDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.LeaseStartDate = LeaseStartDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.LeaseEndDate = LeaseEndDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.RentAmount = double.Parse(RentAmountTextBox.Text);
            selectedTenant.RentPaymentFrequency = (RentPaymentFrequencyComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            selectedTenant.IsActiveTenant = IsActiveTenantCheckBox.IsChecked.Value;

            try
            {
                await UpdateTenantInFirebaseAsync(selectedTenant);
                DisplayDialog("Success", "New Tenant has been updated.");
                GlobalData.tenant = selectedTenant;
                Frame.Navigate(typeof(TenantDetails));
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

        private async Task UpdateTenantInFirebaseAsync(TenantItem updatedTenant)
        {
            // Serialize the updated property
            string json = JsonConvert.SerializeObject(updatedTenant);

            // Create an HttpContent object with the serialized JSON data
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Use the Firebase Realtime Database URL with the property's ID
            string firebaseUrl = $"{GlobalData.firebaseDatabase}/tenants/{updatedTenant.Id}.json?auth={GlobalData.firebaseAuthentication}";

            using (var httpClient = new HttpClient())
            {
                // Send a PUT request to update the property data in the Firebase Database
                HttpResponseMessage response = await httpClient.PutAsync(firebaseUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response
                    DisplayDialog("Error", "Failed to update property data");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TenantDetails));
        }
    }
}
