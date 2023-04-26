using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PropertyDetails : Page
    {
        private PropertyItem _selectedProperty;

        public PropertyDetails()
        {
            this.InitializeComponent();
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


        private void TenantsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProperty != null)
            {
                Frame.Navigate(typeof(TenantsList), _selectedProperty);
            }
            else
            {
                // If _selectedProperty is null, display an error message or handle the situation appropriately
                DisplayDialog("Error", "No property selected.");
            }
        }

        private void MaintenanceAndRepairButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MaintenanceAndRepair), _selectedProperty);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Edit Property page or implement the editing functionality here
            Frame.Navigate(typeof(EditProperty));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProperty != null)
            {
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Confirm Delete",
                    Content = "Are you sure you want to delete this property?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };

                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    await DeletePropertyFromFirebaseDatabaseAsync(_selectedProperty.Id);
                    Frame.Navigate(typeof(Property));
                }
            }
        }


        private async Task DeletePropertyFromFirebaseDatabaseAsync(string propertyId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}properties/{propertyId}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response
                    DisplayDialog("Error", "Failed to delete property data");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Property));
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProperty != null)
            {
                Frame.Navigate(typeof(AppointmentList), _selectedProperty);
            }
            else
            {
                // If _selectedProperty is null, display an error message or handle the situation appropriately
                DisplayDialog("Error", "No property selected.");
            }
        }
    }

}
