using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
<<<<<<< HEAD
=======
using Windows.Storage;
using Windows.Storage.Pickers;
>>>>>>> main2
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
<<<<<<< HEAD
=======
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Data.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
>>>>>>> main2

namespace PropertyManagement
{
    public sealed partial class AddTenant : Page
    {
        private PropertyItem _selectedProperty;
<<<<<<< HEAD
=======
        private StorageFile _selectedContractFile;
>>>>>>> main2

        public AddTenant()
        {
            this.InitializeComponent();
        }
<<<<<<< HEAD


=======
>>>>>>> main2
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PhoneNumberTextBox.Text) ||
                DateOfBirthDatePicker.SelectedDate == null ||
                LeaseStartDatePicker.SelectedDate == null ||
                LeaseEndDatePicker.SelectedDate == null ||
                string.IsNullOrEmpty(RentAmountTextBox.Text) ||
                RentPaymentFrequencyComboBox.SelectedItem == null)
            {
                DisplayDialog("Invalid Input", "Please Enter all necessary details");
                return;
            }

            _selectedProperty = GlobalData.property;

<<<<<<< HEAD
=======
            string contractFileUrl = await UploadContractFileToFirebaseStorageAsync(_selectedContractFile);


>>>>>>> main2
            TenantItem tenant = new TenantItem
            {
                Id = "",
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                DateOfBirth = DateOfBirthDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                PropertyId = _selectedProperty.Id,
                LeaseStartDate = LeaseStartDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                LeaseEndDate = LeaseEndDatePicker.SelectedDate.Value.Date.ToString("yyyy-MM-dd"),
                RentAmount = double.Parse(RentAmountTextBox.Text),
                RentPaymentFrequency = (RentPaymentFrequencyComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
<<<<<<< HEAD
                IsActiveTenant = IsActiveTenantCheckBox.IsChecked.Value
=======
                IsActiveTenant = IsActiveTenantCheckBox.IsChecked.Value,
                ContractFile = contractFileUrl
>>>>>>> main2
            };

            string firebaseKey = await CreateTenantInFirebaseDatabaseAsync(tenant);

            if (firebaseKey != null)
            {
                tenant.Id = firebaseKey;

                try
                {
                    await UpdateTenantInFirebaseDatabaseAsync(firebaseKey, tenant);
                    DisplayDialog("Success", "New Tenant has been added.");
                    Frame.Navigate(typeof(TenantsList));
                }
                catch (Exception ex)
                {
                    DisplayDialog("Error", ex.Message);
                }

            }

        }
<<<<<<< HEAD

=======
>>>>>>> main2
        private async Task<string> CreateTenantInFirebaseDatabaseAsync(TenantItem tenant)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(tenant);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants.json?auth={GlobalData.firebaseAuthentication}");
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

<<<<<<< HEAD
=======
        private async void ContractFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                FileTypeFilter = { ".pdf"}
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

>>>>>>> main2
        private async Task UpdateTenantInFirebaseDatabaseAsync(string firebaseKey, TenantItem tenant)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(tenant);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Uri requestUri = new Uri($"{GlobalData.firebaseDatabase}tenants/{firebaseKey}.json?auth={GlobalData.firebaseAuthentication}");
                HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message);
            }
        }

<<<<<<< HEAD
=======
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


>>>>>>> main2
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TenantsList));
        }
    }
}
