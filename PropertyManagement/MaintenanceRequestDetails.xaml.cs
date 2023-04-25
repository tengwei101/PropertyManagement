﻿using Newtonsoft.Json;
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

namespace PropertyManagement
{
    public sealed partial class MaintenanceRequestDetails : Page
    {
        private MaintenanceRequest _selectedRequest;

        public MaintenanceRequestDetails()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (GlobalData.maintenanceRequest != null)
            {
                DataContext = GlobalData.maintenanceRequest;
                _selectedRequest = GlobalData.maintenanceRequest;
            }
        }

        private async void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string newStatus = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            _selectedRequest = GlobalData.maintenanceRequest;

            if (newStatus != _selectedRequest.Status)
            {
                _selectedRequest.Status = newStatus;

                if (newStatus == "Completed")
                {
                    _selectedRequest.CompletionDate = DateTime.Now.ToString();
                }
                else
                {
                    _selectedRequest.CompletionDate = null;
                }

                await UpdateRequestInFirebaseDatabaseAsync(_selectedRequest.Id, _selectedRequest);
            }
        }

        private async Task UpdateRequestInFirebaseDatabaseAsync(string requestId, MaintenanceRequest request)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests/{requestId}.json?auth={GlobalData.firebaseAuthentication}");

                string jsonData = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditMaintenanceRequest));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest != null)
            {
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Confirm Delete",
                    Content = "Are you sure you want to delete this request?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };

                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    await DeleteRequestFromFirebaseDatabaseAsync(_selectedRequest.Id);
                    Frame.Navigate(typeof(MaintenanceAndRepair));
                }
            }
        }


        private async Task DeleteRequestFromFirebaseDatabaseAsync(string requestId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests/{requestId}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response
                    DisplayDialog("Error", "Failed to delete request data");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MaintenanceAndRepair));
        }
    }
}
