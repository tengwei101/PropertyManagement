using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using System.Text.RegularExpressions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMaintenanceRequest : Page

    {
        private StorageFile _selectedImage;
        private MaintenanceRequest maintenanceRequest;
        private bool completionDatePickerChanged = false;
        private bool submissionDatePickerChanged = false;

        public EditMaintenanceRequest()
        {
            this.InitializeComponent();
            LoadMaintenanceRequestData();
        }

        private void LoadMaintenanceRequestData()
        {
            maintenanceRequest = GlobalData.maintenanceRequest;

            TenantNameTextBox.Text = maintenanceRequest.TenantName;
            DescriptionTextBox.Text = maintenanceRequest.Description;
            SetPriority(maintenanceRequest.Priority);
            SetStatus(maintenanceRequest.Status);

            if(maintenanceRequest.TenantPhone != null)
            {
                TenantPhoneTextBox.Text = maintenanceRequest.TenantPhone;
            }
            else
            {
                TenantPhoneTextBox.Text = string.Empty;
            }

            if (DateTime.TryParse(maintenanceRequest.SubmissionDate, out DateTime submissionDate))
            {
                SubmissionDatePicker.Date = submissionDate;
            }
            if (DateTime.TryParse(maintenanceRequest.CompletionDate, out DateTime completionDate))
            {
                CompletionDatePicker.Date = completionDate;
            }

            if (!string.IsNullOrEmpty(maintenanceRequest.ImageUrl))
            {
                LoadImage(maintenanceRequest.ImageUrl);
            }
        }

        private void SetPriority(string priority)
        {
            foreach (ComboBoxItem item in PriorityComboBox.Items)
            {
                if (item.Content.ToString() == priority)
                {
                    PriorityComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SetStatus(string status)
        {
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private async void LoadImage(string imageUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                using (Stream imageStream = await response.Content.ReadAsStreamAsync())
                {
                    BitmapImage image = new BitmapImage();
                    await image.SetSourceAsync(imageStream.AsRandomAccessStream());
                    PreviewImage.Source = image;
                }
            }
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = _selectedImage != null ? await UploadImageToFirebaseStorageAsync(_selectedImage) : maintenanceRequest.ImageUrl;
            string tenantName = TenantNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string tenanatPhoneNumber = TenantPhoneTextBox.Text;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                maintenanceRequest.ImageUrl = imageUrl;
            }

            string completeDate;
            if (!completionDatePickerChanged)
            {
                completeDate = maintenanceRequest.CompletionDate;
            }
            else
            {
                completeDate = CompletionDatePicker.Date.ToString();
            }

            string submissionDate;
            if (!submissionDatePickerChanged)
            {
                submissionDate = maintenanceRequest.SubmissionDate;
            }
            else
            {
                submissionDate = SubmissionDatePicker.Date.ToString();
            }

            if (string.IsNullOrWhiteSpace(tenantName) || string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(priority) || string.IsNullOrWhiteSpace(status))
            {
                DisplayDialog("Error", "All fields are required.");
                return;
            }

            string phoneNumber = TenantPhoneTextBox.Text;
            string phonePattern = @"^01\d{8,9}$";

            if (!Regex.IsMatch(phoneNumber, phonePattern))
            {
                DisplayDialog("Invalid Input", "Phone number format is incorrect. Please enter a valid phone number (01XXXXXXXX).");
                return;
            }


            MaintenanceRequest updatedRequest = new MaintenanceRequest
            {
                Id = GlobalData.maintenanceRequest.Id,
                TenantName = tenantName,
                TenantPhone = tenanatPhoneNumber,
                Description = description,
                Priority = priority,
                Status = status,
                SubmissionDate = submissionDate,
                CompletionDate = completeDate,
                ImageUrl = imageUrl, // Assuming the image is not changed
                PropertyId = GlobalData.property.Id

            };

            try
            {
                await UpdateRequestInFirebaseAsync(updatedRequest);
                DisplayDialog("Success", "Maintenance request has been updated.");
                Frame.Navigate(typeof(MaintenanceRequestDetails));
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
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

        private async Task UpdateRequestInFirebaseAsync(MaintenanceRequest updatedRequest)
        {
            HttpClient httpClient = new HttpClient();
            Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}maintenance_requests/{updatedRequest.Id}.json?auth={GlobalData.firebaseAuthentication}");

            string json = JsonConvert.SerializeObject(updatedRequest);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MaintenanceRequestDetails));
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
    }
}
