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
    public sealed partial class MaintenanceAndRepair : Page
    {

        public MaintenanceAndRepair()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadMaintenanceRequestsAsync();
        }

        private async Task LoadMaintenanceRequestsAsync(string requestStatusFilter = "All")
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests.json?auth={GlobalData.firebaseAuthentication}");

                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Dictionary<string, MaintenanceRequest> responseData = JsonConvert.DeserializeObject<Dictionary<string, MaintenanceRequest>>(responseBody);

                // Assign request IDs from the dictionary keys and create a list of tenants
                List<MaintenanceRequest> requests = responseData.Select(kvp =>
                {
                    kvp.Value.Id = kvp.Key;
                    return kvp.Value;
                }).ToList();

                // Filter tenants based on the tenantStatusFilter
                if (requestStatusFilter != "All")
                {
                    requests = requests.Where(t => t.Status == requestStatusFilter).ToList();
                }


                requests = requests.Where(t => t.PropertyId == GlobalData.property.Id).ToList();

                // Set the ItemsSource property of the TenantListView
                MaintenanceRequestView.ItemsSource = requests;
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddMaintenanceRequest));
        }

        private async void RequestStatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string statusFilter = (RequestStatusFilterComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            await LoadMaintenanceRequestsAsync(statusFilter);
        }

        private void MaintenanceRequestView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaintenanceRequestView.SelectedItem is MaintenanceRequest selectedRequest)
            {
                // Navigate to the PropertyDetails page with the selected property
                GlobalData.maintenanceRequest = selectedRequest;
                Frame.Navigate(typeof(MaintenanceRequestDetails));

                // Clear the selection
                MaintenanceRequestView.SelectedItem = null;
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
    }
}
