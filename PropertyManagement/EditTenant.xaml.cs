using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditTenant : Page
    {
        private TenantItem selectedTenant;
        private StorageFile _selectedContractFile;

        public EditTenant()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            selectedTenant = GlobalData.tenant;

            if (selectedTenant != null)
            {
                NameTextBox.Text = selectedTenant.Name;
                EmailTextBox.Text = selectedTenant.Email;
                PhoneNumberTextBox.Text = selectedTenant.PhoneNumber;
                DateOfBirthDatePicker.Date = DateTimeOffset.Parse(selectedTenant.DateOfBirth);
                LeaseStartDatePicker.Date = DateTimeOffset.Parse(selectedTenant.LeaseStartDate);
                LeaseEndDatePicker.Date = DateTimeOffset.Parse(selectedTenant.LeaseEndDate);
                RentAmountTextBox.Text = selectedTenant.RentAmount.ToString();
                SetRentPaymentFrequency(selectedTenant.RentPaymentFrequency);
                IsActiveTenantCheckBox.IsChecked = selectedTenant.IsActiveTenant;
                LoadContractFileAsync(selectedTenant.ContractFile);
            }
        }

        private async Task LoadContractFileAsync(string contractFileUrl)
        {
            if (!string.IsNullOrEmpty(contractFileUrl))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(contractFileUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                        {
                            await response.Content.CopyToAsync(stream.AsStreamForWrite());

                            PdfDocument pdfDocument = await PdfDocument.LoadFromStreamAsync(stream);
                            if (pdfDocument.PageCount > 0)
                            {
                                using (PdfPage pdfPage = pdfDocument.GetPage(0))
                                {
                                    BitmapImage imageSource = new BitmapImage();
                                    using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
                                    {
                                        await pdfPage.RenderToStreamAsync(memoryStream);
                                        await imageSource.SetSourceAsync(memoryStream);
                                    }
                                    ContractFileImage.Source = imageSource;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetRentPaymentFrequency(string rentPaymentFrequency)
        {
            foreach (ComboBoxItem item in RentPaymentFrequencyComboBox.Items)
            {
                if (item.Content.ToString() == rentPaymentFrequency)
                {
                    RentPaymentFrequencyComboBox.SelectedItem = item;
                    break;
                }
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

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string contractFile = _selectedContractFile != null ? await UploadContractFileToFirebaseStorageAsync(_selectedContractFile) : selectedTenant.ContractFile;
            // Update the selectedTenant object with the new values from the controls
            selectedTenant.Name = NameTextBox.Text;
            selectedTenant.Email = EmailTextBox.Text;
            selectedTenant.PhoneNumber = PhoneNumberTextBox.Text;
            selectedTenant.DateOfBirth = DateOfBirthDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.LeaseStartDate = LeaseStartDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.LeaseEndDate = LeaseEndDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            selectedTenant.RentAmount = double.Parse(RentAmountTextBox.Text);
            selectedTenant.RentPaymentFrequency = (RentPaymentFrequencyComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            selectedTenant.IsActiveTenant = IsActiveTenantCheckBox.IsChecked.Value;
            if (!string.IsNullOrEmpty(contractFile))
            {
                selectedTenant.ContractFile = contractFile;
            }

            try
            {
                await UpdateTenantInFirebaseAsync(selectedTenant);
                DisplayDialog("Success", "New Tenant has been updated.");
                GlobalData.tenant = selectedTenant;
                Frame.Navigate(typeof(TenantDetails));
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

        private async Task UpdateTenantInFirebaseAsync(TenantItem updatedTenant)
        {
            // Serialize the updated property
            string json = JsonConvert.SerializeObject(updatedTenant);

            // Create an HttpContent object with the serialized JSON data
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Use the Firebase Realtime Database URL with the property's ID
            string firebaseUrl = $"{GlobalData.firebaseDatabase}/tenants/{updatedTenant.Id}.json?auth={GlobalData.firebaseAuthentication}";

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

        private async void ContractFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                FileTypeFilter = { ".pdf" }
            };

            _selectedContractFile = await picker.PickSingleFileAsync();
            if (_selectedContractFile != null)
            {
                SelectedContractFileName.Text = _selectedContractFile.Name;

                // Display the first page of the PDF file
                using (IRandomAccessStreamWithContentType stream = await _selectedContractFile.OpenReadAsync())
                {
                    PdfDocument pdfDocument = await PdfDocument.LoadFromStreamAsync(stream);
                    if (pdfDocument.PageCount > 0)
                    {
                        using (PdfPage pdfPage = pdfDocument.GetPage(0))
                        {
                            BitmapImage imageSource = new BitmapImage();
                            using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
                            {
                                await pdfPage.RenderToStreamAsync(memoryStream);
                                await imageSource.SetSourceAsync(memoryStream);
                            }
                            ContractFileImage.Source = imageSource;
                        }
                    }
                }
            }
        }

        private async Task<string> UploadContractFileToFirebaseStorageAsync(StorageFile contractFile)
        {
            if (contractFile == null)
            {
                return "";
            }

            string fileUrl = "";
            string fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{contractFile.Name}";

            try
            {
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri($"https://firebasestorage.googleapis.com/v0/b/{GlobalData.firebaseStorage}/o/{Uri.EscapeDataString(fileName)}?uploadType=media&name={Uri.EscapeDataString(fileName)}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Firebase", GlobalData.firebaseAuthentication);

                using (var fileStream = await contractFile.OpenStreamForReadAsync())
                {
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StreamContent(fileStream));
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        string downloadTokens = responseJson.Value<string>("downloadTokens");
                        fileUrl = $"https://firebasestorage.googleapis.com/v0/b/{GlobalData.firebaseStorage}/o/{Uri.EscapeDataString(fileName)}?alt=media&token={downloadTokens}";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }

            return fileUrl;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TenantDetails));
        }
    }
}
