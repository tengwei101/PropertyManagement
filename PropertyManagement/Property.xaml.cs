using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Property : Page
    {
        private FirebaseAuthHelper _authHelper;
        public ObservableCollection<Property> FilteredProperty { get; set; } = new ObservableCollection<Property>();

        public Property()
        {
            this.InitializeComponent();
            LoadPropertiesAsync();
            _authHelper = new FirebaseAuthHelper(GlobalData.firebaseAuthentication);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddProperty));
        }

        private async Task LoadPropertiesAsync(string propertyStatusFilter = null, string searchText = null)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}properties.json?auth={GlobalData.firebaseAuthentication}");

                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var propertiesDictionary = JsonConvert.DeserializeObject<Dictionary<string, PropertyItem>>(responseBody);

                List<PropertyItem> properties = new List<PropertyItem>();

                if (propertyStatusFilter == null || propertyStatusFilter == "All")
                {
                    properties = propertiesDictionary.Values.ToList();
                }
                else
                {
                    properties = propertiesDictionary.Values.Where(p => p.PropertyStatus == propertyStatusFilter).ToList();
                }

                if (!string.IsNullOrEmpty(searchText))
                {
                    properties = properties.Where(p => p.PropertyName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }


                PropertyListView.ItemsSource = properties;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Notification", "There is no property in the database.");
            }
        }

        private void PropertyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertyListView.SelectedItem is PropertyItem selectedProperty)
            {
                // Navigate to the PropertyDetails page with the selected property
                GlobalData.property = selectedProperty;
                Frame.Navigate(typeof(PropertyDetails));

                // Clear the selection
                PropertyListView.SelectedItem = null;
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

        private async void PropertyStatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            string propertyStatusFilter = selectedItem.Content.ToString();

            string searchText = SearchTextBox.Text;

            await LoadPropertiesAsync(propertyStatusFilter, searchText);
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the logout confirmation dialog and check if the user confirms
            bool shouldLogout = await ShowLogoutConfirmationDialog();

            if (shouldLogout)
            {
                // Call the SignOut method to remove the authentication token and user information
                _authHelper.SignOut();

                // Navigate back to the login page
                Frame.Navigate(typeof(MainPage)); // Replace LoginPage with the name of your actual login page class
            }
        }


        private async Task<bool> ShowLogoutConfirmationDialog()
        {
            ContentDialog logoutConfirmationDialog = new ContentDialog
            {
                Title = "Confirm Logout",
                Content = "Are you sure you want to log out?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await logoutConfirmationDialog.ShowAsync();

            return result == ContentDialogResult.Primary;
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            string searchText = searchTextBox.Text;

            string propertyStatusFilter = ((ComboBoxItem)PropertyStatusFilterComboBox.SelectedItem).Content.ToString();

            await LoadPropertiesAsync(propertyStatusFilter, searchText);
        }

    }
}
