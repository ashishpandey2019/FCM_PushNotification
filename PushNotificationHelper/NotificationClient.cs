using PushNotificationHelper.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PushNotificationHelper
{
    public class NotificationClient
    {
        private readonly HttpClient _httpClient;
        private readonly Action<string> _logger;

        public NotificationClient(HttpClient httpClient, Action<string> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        //public async Task<string> RegsiterNotification(UserTokenModel registerTokenModel)
        //{

        //    return await PostDataAsync(registerTokenModel, "api/Notification/RegsiterNotification");
        //}

        //public async Task<string> UnRegsiterNotification(UserTokenModel userTokenModel)
        //{
        //    return await PostDataAsync(userTokenModel, "api/Notification/UnRegsiterNotification");
        //}

        public async Task<IList<PushNotificationSendResult>> SendNotificationAsync(PushNotificationMessage pushNotificationMessage)
        {
            var result = await PostDataAsync(pushNotificationMessage, "api/Notification/SendNotificationAsync");
            return JsonSerializer.Deserialize<IList<PushNotificationSendResult>>(result);
        }

        public async Task<string> PostDataAsync<TSender>(TSender sender, string Url)
        {
            string content = string.Empty;
            try
            {
                content = JsonSerializer.Serialize(sender);
                using HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                using var response = await _httpClient.PostAsync(Url, httpContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return stringContent;
            }
            catch (Exception ee)
            {
                _logger?.Invoke(ee.ToString());
                return null;
            }
        }
    }
}