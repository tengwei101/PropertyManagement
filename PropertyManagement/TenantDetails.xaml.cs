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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TenantDetails : Page
    {
        private TenantItem _selectedTenant;

        public TenantDetails()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Use GlobalData.tenant to get the selected tenant
            TenantItem selectedTenant = GlobalData.tenant;

            if (selectedTenant != null)
            {
                this.DataContext = selectedTenant;
                _selectedTenant = selectedTenant;
                // Set the IsChecked property of the ActiveCheckBox based on the IsActiveTenant value
                ActiveCheckBox.IsChecked = _selectedTenant.IsActiveTenant;
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditTenant));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTenant != null)
            {
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Confirm Delete",
                    Content = "Are you sure you want to delete this tenant?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };

                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    await DeleteTenantFromFirebaseDatabaseAsync(_selectedTenant.Id);
                    Frame.Navigate(typeof(TenantsList));
                }
            }
        }


        private async Task DeleteTenantFromFirebaseDatabaseAsync(string tenantId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants/{tenantId}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response
                    DisplayDialog("Error", "Failed to delete tenant data");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
            }
        }

        private async Task UpdateTenantIsActiveStatusAsync(string tenantId, bool isActive)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants/{tenantId}/IsActiveTenant.json?auth={GlobalData.firebaseAuthentication}");
                StringContent content = new StringContent(isActive.ToString().ToLower(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response
                    DisplayDialog("Error", "Failed to update tenant's IsActiveTenant status.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
            }
        }


        private async void ActiveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            if (activeCheckBox != null && _selectedTenant != null)
            {
                await UpdateTenantIsActiveStatusAsync(_selectedTenant.Id, true);
            }
        }

        private async void ActiveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            if (activeCheckBox != null && _selectedTenant != null)
            {
                await UpdateTenantIsActiveStatusAsync(_selectedTenant.Id, false);
            }
        }


        private async void DisplayDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
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
