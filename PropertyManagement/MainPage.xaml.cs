using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase.Auth.Providers;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PropertyManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FirebaseAuthHelper _auth;
        private string _firebaseApiKey = GlobalData.firebaseAuthentication;

        public MainPage()
        {
            this.InitializeComponent();
            _auth = new FirebaseAuthHelper(GlobalData.firebaseAuthentication);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await SignInAsync(EmailTextBox.Text, PasswordBox.Password);
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            await CreateUserAsync(EmailTextBox.Text, PasswordBox.Password);
        }

        private async Task SignInAsync(string email, string password)
        {
            try
            {
                var auth = await _auth.SignInAsync(email, password);
                StatusTextBlock.Text = $"Logged in successfully: {auth.Email}";
                Frame.Navigate(typeof(Property));

            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Login failed: {ex.Message}";
            }
        }

        private async Task CreateUserAsync(string email, string password)
        {
            try
            {
                var auth = await _auth.CreateUserAsync(email, password);
                StatusTextBlock.Text = $"User registered successfully: {auth.Email}";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Registration failed: {ex.Message}";
            }
        }

    }


}
