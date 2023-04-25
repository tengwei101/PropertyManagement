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
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditProperty : Page
    {
        private PropertyItem _propertyToEdit;
        private StorageFile _selectedImage;


        public EditProperty()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get the property details from GlobalData.property
            _propertyToEdit = GlobalData.property;

            if (_propertyToEdit != null)
            {
                // Set the values of the controls with the property details
                PropertyNameTextBox.Text = _propertyToEdit.PropertyName;
                PropertyTypeComboBox.SelectedItem = GetComboBoxItemByContent(PropertyTypeComboBox, _propertyToEdit.PropertyType);
                AddressTextBox.Text = _propertyToEdit.Address;
                BedroomsTextBox.Text = _propertyToEdit.NumberOfBedrooms.ToString();
                BathroomsTextBox.Text = _propertyToEdit.NumberOfBathrooms.ToString();
                SquareFeetTextBox.Text = _propertyToEdit.SquareFeet.ToString();
                DescriptionTextBox.Text = _propertyToEdit.Description;
                PropertyStatusComboBox.SelectedItem = GetComboBoxItemByContent(PropertyStatusComboBox, _propertyToEdit.PropertyStatus);
                OwnerTextBox.Text = _propertyToEdit.Owner;
                PriceTextBox.Text = _propertyToEdit.Price.ToString();
                LoadImageFromUrlAsync(_propertyToEdit.ImageUrl);
            }
        }

        private ComboBoxItem GetComboBoxItemByContent(ComboBox comboBox, string content)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == content)
                {
                    return item;
                }
            }

            return null;
        }


        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = _selectedImage != null ? await UploadImageToFirebaseStorageAsync(_selectedImage) : _propertyToEdit.ImageUrl;
            // Update the _propertyToEdit object with the new values from the controls
            _propertyToEdit.PropertyName = PropertyNameTextBox.Text;
            _propertyToEdit.PropertyType = (PropertyTypeComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            _propertyToEdit.Address = AddressTextBox.Text;
            _propertyToEdit.NumberOfBedrooms = int.Parse(BedroomsTextBox.Text);
            _propertyToEdit.NumberOfBathrooms = int.Parse(BathroomsTextBox.Text);
            _propertyToEdit.SquareFeet = int.Parse(SquareFeetTextBox.Text);
            _propertyToEdit.Description = DescriptionTextBox.Text;
            _propertyToEdit.PropertyStatus = (PropertyStatusComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            _propertyToEdit.Owner = OwnerTextBox.Text;
            _propertyToEdit.Price = double.Parse(PriceTextBox.Text);

            if (!string.IsNullOrEmpty(imageUrl))
            {
                _propertyToEdit.ImageUrl = imageUrl;
            }

            try
            {
                await UpdatePropertyInFirebaseAsync(_propertyToEdit);
                DisplayDialog("Success", "Property has been updated.");
                GlobalData.property = _propertyToEdit;
                Frame.Navigate(typeof(PropertyDetails));
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

        private async Task LoadImageFromUrlAsync(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(imageUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            using (InMemoryRandomAccessStream imageStream = new InMemoryRandomAccessStream())
                            {
                                await response.Content.CopyToAsync(imageStream.AsStreamForWrite()).ConfigureAwait(false);
                                await imageStream.FlushAsync(); // Removed ConfigureAwait(false) here
                                imageStream.Seek(0);

                                BitmapImage bitmapImage = new BitmapImage();
                                await bitmapImage.SetSourceAsync(imageStream);
                                PropertyImageView.Source = bitmapImage;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayDialog("Error", ex.Message);
                }
            }
        }



        private async Task UpdatePropertyInFirebaseAsync(PropertyItem updatedProperty)
        {
            // Serialize the updated property
            string json = JsonConvert.SerializeObject(updatedProperty);

            // Create an HttpContent object with the serialized JSON data
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Use the Firebase Realtime Database URL with the property's ID
            string firebaseUrl = $"{GlobalData.firebaseDatabase}/properties/{updatedProperty.Id}.json?auth={GlobalData.firebaseAuthentication}";

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


        private async void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            _selectedImage = await picker.PickSingleFileAsync();
            if (_selectedImage != null)
            {
                using (IRandomAccessStream fileStream = await _selectedImage.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    PropertyImageView.Source = bitmapImage;
                    // You can add an Image control in the XAML to display the image
                }
            }
        }
        private async Task<string> UploadImageToFirebaseStorageAsync(StorageFile image)
        {
            if (image == null)
            {
                return "";
            }

            string imageUrl = "";
            string fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{image.Name}";

            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"https://firebasestorage.googleapis.com/v0/b/{GlobalData.firebaseStorage}/o/{Uri.EscapeDataString(fileName)}?uploadType=media&name={Uri.EscapeDataString(fileName)}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Firebase", GlobalData.firebaseAuthentication);

                using (var imageStream = await image.OpenStreamForReadAsync())
                {
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StreamContent(imageStream));
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        string downloadTokens = responseJson.Value<string>("downloadTokens");
                        imageUrl = $"https://firebasestorage.googleapis.com/v0/b/{GlobalData.firebaseStorage}/o/{Uri.EscapeDataString(fileName)}?alt=media&token={downloadTokens}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayDialog("Error", ex.Message);
            }

            return imageUrl;
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
            Frame.Navigate(typeof(PropertyDetails));
        }
    }

}
