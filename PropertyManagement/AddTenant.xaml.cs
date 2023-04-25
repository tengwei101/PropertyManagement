using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PropertyManagement
{
    public sealed partial class AddTenant : Page
    {
        private PropertyItem _selectedProperty;

        public AddTenant()
        {
            this.InitializeComponent();
        }


        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PhoneNumberTextBox.Text) ||
                DateOfBirthDatePicker.SelectedDate == null ||
                LeaseStartDatePicker.SelectedDate == null ||
                LeaseEndDatePicker.SelectedDate == null ||
                string.IsNullOrEmpty(RentAmountTextBox.Text) ||
                RentPaymentFrequencyComboBox.SelectedItem == null)
            {
                DisplayDialog("Invalid Input", "Please Enter all necessary details");
                return;
            }

            _selectedProperty = GlobalData.property;

            TenantItem tenant = new TenantItem
            {
                Id = "",
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                DateOfBirth = DateOfBirthDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                PropertyId = _selectedProperty.Id,
                LeaseStartDate = LeaseStartDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                LeaseEndDate = LeaseEndDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                RentAmount = double.Parse(RentAmountTextBox.Text),
                RentPaymentFrequency = (RentPaymentFrequencyComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                IsActiveTenant = IsActiveTenantCheckBox.IsChecked.Value
            };

            string firebaseKey = await CreateTenantInFirebaseDatabaseAsync(tenant);

            if (firebaseKey != null)
            {
                tenant.Id = firebaseKey;

                try
                {
                    await UpdateTenantInFirebaseDatabaseAsync(firebaseKey, tenant);
                    DisplayDialog("Success", "New Tenant has been added.");
                    Frame.Navigate(typeof(TenantsList));
                }
                catch (Exception ex)
                {
                    DisplayDialog("Error", ex.Message);
                }

            }

        }

        private async Task<string> CreateTenantInFirebaseDatabaseAsync(TenantItem tenant)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(tenant);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants.json?auth={GlobalData.firebaseAuthentication}");
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

        private async Task UpdateTenantInFirebaseDatabaseAsync(string firebaseKey, TenantItem tenant)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(tenant);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants/{firebaseKey}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

        private async void DisplayDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TenantsList));
        }
    }
}
