
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSupermarket.WinForms
{
    public static class ApiClientService
    {
        //============================================================================40: Dùng chung HttpClient
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5138/api/")
        };
        
        public static HttpClient Client
        {
            get
            {
                SetAuthorization();
                return _client;
            }
        }
        //============================================================================38
        private static void SetAuthorization()
        {
            _client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(SessionManager.JwtToken))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }
        }

        public static async Task<bool> LoginAsync(string username, string password)
        {
            var loginObj = new
            {
                Username = username,
                Password = password
            };

            var response = await _client.PostAsJsonAsync("auth/login", loginObj);

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonString);
            //========================================================================38
            SessionManager.JwtToken =
                doc.RootElement.GetProperty("token").GetString() ?? string.Empty;
            SessionManager.CurrentRole =
                doc.RootElement.GetProperty("role").GetString() ?? string.Empty;

            SessionManager.CurrentUsername = username;

            return true;
        }

        public static async Task<string> GetDataWithTokenAsync(string endpoint)
        {
            //==================================================================================40
            SetAuthorization();

            var response = await _client.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new Exception("Phiên làm việc hết hạn hoặc chưa đăng nhập!");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Lỗi khi lấy dữ liệu từ Server: " + response.StatusCode);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<bool> PostDataWithTokenAsync<T>(
            string endpoint, T data)
        {
            SetAuthorization();

            var response = await _client.PostAsJsonAsync(endpoint, data);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> PutDataWithTokenAsync<T>(
            string endpoint, T data)
        {
            SetAuthorization();

            var response = await _client.PutAsJsonAsync(endpoint, data);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteDataWithTokenAsync(string endpoint)
        {
            SetAuthorization();

            var response = await _client.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }

        public static async Task<(bool Success, string Message)> PostDataWithTokenMessageAsync<T>(
            string endpoint, T data)
        {
            SetAuthorization();

            var response = await _client.PostAsJsonAsync(endpoint, data);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Thêm danh mục thành công!");
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(responseBody);

                if (document.RootElement.TryGetProperty("message", out var message))
                {
                    return (false, message.GetString() ?? "Thao tác thất bại!");
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // Phản hồi không phải JSON hợp lệ.
            }

            return (false, "Thao tác thất bại!");
        }

    }
}