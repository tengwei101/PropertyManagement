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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TenantsList : Page
    {
        private PropertyItem _selectedProperty;

        public TenantsList()
        {
            this.InitializeComponent();
            LoadTenantsAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Use GlobalData.property to get the selected property
            PropertyItem selectedProperty = GlobalData.property;

            if (selectedProperty != null)
            {
                this.DataContext = selectedProperty;
                _selectedProperty = selectedProperty;
                // TODO: Display the selected property details in the PropertyDetails page
            }
        }


        private async Task LoadTenantsAsync(string tenantStatusFilter = null, string propertyId = null)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Dictionary<string, TenantItem> responseData = JsonConvert.DeserializeObject<Dictionary<string, TenantItem>>(responseBody);

                // Assign tenant IDs from the dictionary keys and create a list of tenants
                List<TenantItem> tenants = responseData.Select(kvp =>
                {
                    kvp.Value.Id = kvp.Key;
                    return kvp.Value;
                }).ToList();

                // Filter tenants based on the tenantStatusFilter
                if (tenantStatusFilter == "Active")
                {
                    tenants = tenants.Where(t => t.IsActiveTenant).ToList();
                }
                else if (tenantStatusFilter == "Inactive")
                {
                    tenants = tenants.Where(t => !t.IsActiveTenant).ToList();
                }

                tenants = tenants.Where(t => t.PropertyId == _selectedProperty.Id).ToList();

                // Set the ItemsSource property of the TenantListView
                TenantListView.ItemsSource = tenants;
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProperty != null)
            {
                Frame.Navigate(typeof(AddTenant));
            }
            else
            {
                // If _selectedProperty is null, display an error message or handle the situation appropriately
                DisplayDialog("Error", "No property selected.");
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
            Frame.Navigate(typeof(PropertyDetails));
        }

        private void TenantListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TenantListView.SelectedItem is TenantItem selectedTenant)
            {
                // Navigate to the PropertyDetails page with the selected property
                GlobalData.tenant = selectedTenant;
                Frame.Navigate(typeof(TenantDetails));

                // Clear the selection
                TenantListView.SelectedItem = null;
            }
        }

        private async void ActiveFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            string selectedItem = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedItem == null) return;

            await LoadTenantsAsync(selectedItem);
        }

    }
}
