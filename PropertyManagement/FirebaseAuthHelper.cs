using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;


namespace PropertyManagement
{
    public class FirebaseAuthHelper
    {
        private readonly string _apiKey;

        public FirebaseAuthHelper(string apiKey)
        {
            _apiKey = apiKey;
        }



        public async Task<SignInResponse> SignInAsync(string email, string password)
        {
            var request = new SignInRequest { Email = email, Password = password, ReturnSecureToken = true };
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SignInResponse>(responseBody);
            }
        }

        public async Task<SignUpResponse> CreateUserAsync(string email, string password)
        {
            var request = new SignUpRequest { Email = email, Password = password, ReturnSecureToken = true };
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_apiKey}", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SignUpResponse>(responseBody);
            }
        }

        public class SignInRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }

            [JsonProperty("returnSecureToken")]
            public bool ReturnSecureToken { get; set; }
        }

        public class SignInResponse
        {
            [JsonProperty("idToken")]
            public string IdToken { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public class SignUpRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }

            [JsonProperty("returnSecureToken")]
            public bool ReturnSecureToken { get; set; }
        }

        public class SignUpResponse
        {
            [JsonProperty("idToken")]
            public string IdToken { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }


public async Task SendPasswordResetEmailAsync(string email)
{
    try
    {
        using (var client = new HttpClient())
        {
            var request = new { requestType = "PASSWORD_RESET", email = email };
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={_apiKey}", content);
            response.EnsureSuccessStatusCode();
        }
    }
    catch (Exception ex)
    {
        throw ex;
    }
}


    }


}
