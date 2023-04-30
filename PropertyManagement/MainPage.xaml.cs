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
using System.Text.RegularExpressions;



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
            // Validate password requirements
            string password = PasswordBox.Password;
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                DisplayDialog("Invalid Input", "Password must have at least one uppercase letter, one lowercase letter, one digit, and be at least 8 characters long.");
                return;
            }

            await CreateUserAsync(EmailTextBox.Text, password);
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


        private async Task SignInAsync(string email, string password)
        {
            try
            {
                var auth = await _auth.SignInAsync(email, password);
                DisplayDialog("Success", $"Logged in successfully: {auth.Email}");
                Frame.Navigate(typeof(Property));

            }
            catch (Exception ex)
            {
                DisplayDialog("Failed", $"Login failed: Wrong email or password, Please try again.");
            }
        }

        private async Task CreateUserAsync(string email, string password)
        {
            try
            {
                var auth = await _auth.CreateUserAsync(email, password);
                DisplayDialog("Success", $"User registered successfully: {auth.Email}");
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Registration failed: {ex.Message}";
                DisplayDialog("Failed", $"Please ensure the format of email {email} is correct");
            }
        }

        private async void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Password Recovery",
                Content = new TextBox { PlaceholderText = "Enter your email" },
                PrimaryButtonText = "Send Reset Email",
                SecondaryButtonText = "Cancel"
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var email = (dialog.Content as TextBox).Text;
                await SendPasswordResetEmailAsync(email);
            }
        }

        private async Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                await _auth.SendPasswordResetEmailAsync(email);
                DisplayDialog("Success", $"Password reset email sent to {email}.");
            }
            catch (Exception ex)
            {
                DisplayDialog("Failed", $"Failed to send password reset email, Please make sure the email {email} is exist and valid");
            }
        }



    }


}
