using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Firebase.Auth.Providers;
using Firebase.Auth;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PropertyManagement
{
    public sealed partial class AddProperty : Page
    {
        private StorageFile _selectedImage;

        public AddProperty()
        {
            this.InitializeComponent();
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

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Upload the image to Firebase Storage and get the download URL
            string imageUrl = await UploadImageToFirebaseStorageAsync(_selectedImage);

            if (_selectedImage == null ||
                string.IsNullOrEmpty(PropertyNameTextBox.Text) ||
                PropertyTypeComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(AddressTextBox.Text) ||
                string.IsNullOrEmpty(BedroomsTextBox.Text) ||
                string.IsNullOrEmpty(BathroomsTextBox.Text) ||
                string.IsNullOrEmpty(SquareFeetTextBox.Text) ||
                string.IsNullOrEmpty(DescriptionTextBox.Text) ||
                PropertyStatusComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(OwnerTextBox.Text) ||
                string.IsNullOrEmpty(PriceTextBox.Text) || imageUrl.Equals(""))
            {
                // Show an error message if any field is empty
                DisplayDialog("Invalid Input", "Please Enter all necessary details");
            }
            else
            {
                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();


                // Create a property object and populate the fields
                PropertyItem property = new PropertyItem
                {
                    Id = "",
                    PropertyName = PropertyNameTextBox.Text,
                    PropertyType = (PropertyTypeComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    Address = AddressTextBox.Text,
                    NumberOfBedrooms = int.Parse(BedroomsTextBox.Text),
                    NumberOfBathrooms = int.Parse(BathroomsTextBox.Text),
                    SquareFeet = int.Parse(SquareFeetTextBox.Text),
                    Description = DescriptionTextBox.Text,
                    PropertyStatus = (PropertyStatusComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    Owner = OwnerTextBox.Text,
                    Price = double.Parse(PriceTextBox.Text),
                    ImageUrl = imageUrl
                };

                // Save the property object to Firebase Database and get the Firebase key
                string firebaseKey = await CreatePropertyInFirebaseDatabaseAsync(property);

                // Check if the property was successfully saved
                if (firebaseKey != null)
                {
                    // Set the property Id to the firebaseKey
                    property.Id = firebaseKey;

                    // Update the property in Firebase Database with the new Id
                    try
                    {
                        await UpdatePropertyInFirebaseDatabaseAsync(firebaseKey, property);
                        DisplayDialog("Success", "New Property has been added.");
                        Frame.Navigate(typeof(Property));
                    }
                    catch (Exception ex)
                    {
                        DisplayDialog("Error", ex.Message);
                    }
                }

                // Clear the form
                PropertyNameTextBox.Text = "";
                PropertyTypeComboBox.SelectedIndex = -1;
                AddressTextBox.Text = "";
                BedroomsTextBox.Text = "";
                BathroomsTextBox.Text = "";
                SquareFeetTextBox.Text = "";
                DescriptionTextBox.Text = "";
                PropertyStatusComboBox.SelectedIndex = -1;
                OwnerTextBox.Text = "";
                PriceTextBox.Text = "";
                _selectedImage = null;
            }

        }

        private async Task<string> CreatePropertyInFirebaseDatabaseAsync(PropertyItem property)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(property);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}properties.json?auth={GlobalData.firebaseAuthentication}");
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

        private async Task UpdatePropertyInFirebaseDatabaseAsync(string firebaseKey, PropertyItem property)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(property);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}properties/{firebaseKey}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
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
            Frame.Navigate(typeof(Property));
        }
    }

}

