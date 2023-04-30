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

        private bool _isPageLoaded = false;

        public MaintenanceAndRepair()
        {
            this.InitializeComponent();
            this.Loaded += MaintenanceAndRepair_Loaded;
        }

        private async void MaintenanceAndRepair_Loaded(object sender, RoutedEventArgs e)
        {
            _isPageLoaded = true;
            await LoadMaintenanceRequestsAsync();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_isPageLoaded)
            {
                string statusFilter = (RequestStatusFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
                string priorityFilter = (PriorityFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
                await LoadMaintenanceRequestsAsync(statusFilter, priorityFilter);
            }
        }

        private async Task LoadMaintenanceRequestsAsync(string requestStatusFilter = "All", string priorityFilter = "All")
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

                // Filter requests based on the property
                requests = requests.Where(t => t.PropertyId == GlobalData.property.Id).ToList();

                // Filter requests based on the requestStatusFilter
                if (requestStatusFilter != "All")
                {
                    requests = requests.Where(t => t.Status == requestStatusFilter).ToList();
                }

                // Filter requests based on the priorityFilter
                if (priorityFilter != "All")
                {
                    requests = requests.Where(t => t.Priority == priorityFilter).ToList();
                }


                // Set the ItemsSource property of the MaintenanceRequestView
                MaintenanceRequestView.ItemsSource = requests;
            }
            catch (Exception ex)
            {
                DisplayDialog("Notification", "There is no maintainence request in this property.");
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddMaintenanceRequest));
        }

        private async void RequestStatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isPageLoaded || RequestStatusFilterComboBox.SelectedItem == null || PriorityFilterComboBox.SelectedItem == null) return;

            string statusFilter = (RequestStatusFilterComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            string priorityFilter = (PriorityFilterComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            await LoadMaintenanceRequestsAsync(statusFilter, priorityFilter);
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
            ContentDialogResult result = await dialog.ShowAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PropertyDetails));
        }

        private async void PriorityFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestStatusFilterComboBox.SelectedItem == null || PriorityFilterComboBox.SelectedItem == null) return;

            string statusFilter = (RequestStatusFilterComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            string priorityFilter = (PriorityFilterComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            await LoadMaintenanceRequestsAsync(statusFilter, priorityFilter);
        }

    }
}
