using Newtonsoft.Json;
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
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddMaintenanceRequest : Page
    {

        private StorageFile _selectedImage;
        private bool completionDatePickerChanged = false;
        private bool submissionDatePickerChanged = false;


        public AddMaintenanceRequest()
        {
            this.InitializeComponent();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TenantNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                PriorityComboBox.SelectedIndex == -1 ||
                StatusComboBox.SelectedIndex == -1 ||
                !submissionDatePickerChanged)
            {
                DisplayDialog("Invalid Input", "Please Enter all necessary details");
            }
            else
            {
                string imageUrl = await UploadImageToFirebaseStorageAsync(_selectedImage);

                string completeDate;
                if (!completionDatePickerChanged)
                {
                    completeDate = "";
                }
                else
                {
                    completeDate = CompletionDatePicker.Date.ToString();
                }



                try
                {
                    MaintenanceRequest maintenanceRequest = new MaintenanceRequest
                    {
                        Id = "",
                        TenantName = TenantNameTextBox.Text, // Assuming the tenant name text box contains the tenant's ID
                        PropertyId = GlobalData.property.Id,
                        Description = DescriptionTextBox.Text,
                        Priority = (PriorityComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                        Status = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                        SubmissionDate = SubmissionDatePicker.Date.DateTime.ToString(),
                        CompletionDate = completeDate,
                        ImageUrl = imageUrl,
                    };
                    string firebaseKey = await CreateRequestInFirebaseDatabaseAsync(maintenanceRequest);


                    if (firebaseKey != null)
                    {
                        // Set the property Id to the firebaseKey
                        maintenanceRequest.Id = firebaseKey;

                        // Update the property in Firebase Database with the new Id
                        try
                        {
                            await UpdateRequestInFirebaseDatabaseAsync(firebaseKey, maintenanceRequest);
                            DisplayDialog("Success", "New Maintenance request has been added.");
                            Frame.Navigate(typeof(MaintenanceAndRepair));
                        }
                        catch (Exception ex)
                        {
                            DisplayDialog("Error", ex.Message);
                        }
                    }

                    TenantNameTextBox.Text = string.Empty;
                    DescriptionTextBox.Text = string.Empty;
                    PriorityComboBox.SelectedIndex = -1;
                    StatusComboBox.SelectedIndex = -1;
                    SubmissionDatePicker.Date = DateTimeOffset.Now;
                    CompletionDatePicker.Date = DateTimeOffset.Now;
                    PreviewImage.Source = null;

                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    DisplayDialog("Error", ex.Message);
                }
            }
            
        }


        private void SubmissionDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            submissionDatePickerChanged = true;
        }

        private void CompletionDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            completionDatePickerChanged = true;
        }


        private async Task<string> CreateRequestInFirebaseDatabaseAsync(MaintenanceRequest request)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests.json?auth={GlobalData.firebaseAuthentication}");
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

        private async Task UpdateRequestInFirebaseDatabaseAsync(string firebaseKey, MaintenanceRequest request)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests/{firebaseKey}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
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
            Frame.Navigate(typeof(MaintenanceAndRepair));
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
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
                    PreviewImage.Source = bitmapImage;
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

            try
            {
                string fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{image.Name}";
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

    }
}
