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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Property : Page
    {
        public Property()
        {
            this.InitializeComponent();
            LoadPropertiesAsync();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddProperty));
        }

        private async Task LoadPropertiesAsync(string propertyStatusFilter = null)
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

                PropertyListView.ItemsSource = properties;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
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

            await LoadPropertiesAsync(propertyStatusFilter);
        }

    }
}
