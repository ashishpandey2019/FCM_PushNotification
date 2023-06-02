

using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using PushNotificationHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SaConclave.PushNotification
{
    public class PushNotificationService
    {
        private readonly ILogger<PushNotificationService> _logger;
        private readonly IWebHostEnvironment _environment;

        public PushNotificationService(ILogger<PushNotificationService> logger, IWebHostEnvironment environment)
        {
            try
            {
                _environment = environment;

                var stream = Path.Combine(_environment.ContentRootPath, "account.json");

               // var stream = typeof(PushNotificationService).Assembly.GetManifestResourceStream("SaConclave.PushNotification.account.json");

                using StreamReader streamReader = new(stream, encoding: Encoding.UTF8);

                var json = streamReader.ReadToEnd();

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(json)
                });
            }
            catch (Exception ee)
            {
            }
            _logger = logger;
        }

        public async Task<IList<PushNotificationSendResult>> SendNotificationAsync(PushNotificationMessage pushNotification)
        {
            if (pushNotification.Tokens.Count < 1)
            {
                throw new ArgumentException("Parameters are not valid.", nameof(pushNotification.Tokens));
            }


            //if (pushNotification.NotToSent?.Length > 0)
            //{
            //    message.Condition = GetCondition(pushNotification.UserTypes, pushNotification.NotToSent);
            //}
            //else if (pushNotification.UserTypes.Length >= 2)
            //{
            //    message.Condition = GetCondition(pushNotification.UserTypes, pushNotification.NotToSent);
            //}
            //else
            //{
            //    message.Topic = pushNotification.UserTypes[0];
            //}

            var tasks = pushNotification.Tokens.Select(x => SendMessageAsync(x, pushNotification.Title, pushNotification.Body, pushNotification.Data)).ToArray();
            var result = await Task.WhenAll(tasks);

            return result;
        }

        public async Task RegisterTopicsForTokenAsync(string token, params string[] topics)
        {
            foreach (var topic in topics)
            {
                _logger.LogInformation($"Subscribing token for topic= {topic}");
                var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new string[] { token }, topic);
                if (response.Errors.Count > 0)
                {
                    foreach (var error in response.Errors)
                    {
                        _logger.LogInformation(error.Reason);
                    }
                }
                if (response.SuccessCount > 0)
                {
                    _logger.LogInformation($"Subscribing successful for token for topic= {topic}");
                }
            }
        }

        public async Task UnRegisterTopicsForTokenAsync(string token, params string[] topics)
        {
            foreach (var topic in topics)
            {
                _logger.LogInformation("UnSubscribing token for topic= {0}", topic);
                var response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(new string[] { token }, topic);
                if (response.Errors.Count > 0)
                {
                    foreach (var error in response.Errors)
                    {
                        _logger.LogInformation(error.Reason);
                    }
                }
                if (response.SuccessCount > 0)
                {
                    _logger.LogInformation("UNSubscribing successful for token for topic= {0}", topic);
                }
            }
        }

        private async Task<PushNotificationSendResult> SendMessageAsync(string token, string title, string body, IReadOnlyDictionary<string, string>? data)
        {
            try
            {
                var result = await FirebaseMessaging.DefaultInstance.SendAsync(GetMessage(token, body, title, data));
                _logger.LogInformation("Success for Token= {0} and result= {1}", token, result);
                return new PushNotificationSendResult
                { Result = result, Token = token };
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "Error while sending notification and to token=  {0}", token);
                return new PushNotificationSendResult
                { Result = ee.ToString(), Token = token };
            }
        }
        private static Message GetMessage(string token, string body, string title, IReadOnlyDictionary<string, string> data)
        {

            return new Message
            {
                Token = token,
                Notification = new Notification { Body = body, Title = title },
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        ChannelId = "FirebasePushNotificationChannel",
                        Body = body,
                        Title = title
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        CustomData = new Dictionary<string, object> { { "sound", "Enabled" }, { "priority", "high" } }
                    }
                }
            };
        }
        //private string GetCondition(string[] topicsIN, string[] topicsNotIN)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append("(");

        //    int i = 0;
        //    while (true)
        //    {
        //        stringBuilder.Append("'").Append(topicsIN[i]).Append("'").Append(" in topics");

        //        i++;
        //        if (i < topicsIN.Length)
        //        {
        //            stringBuilder.Append(" || ");
        //        }
        //        else
        //        {
        //            stringBuilder.Append(")");
        //            break;
        //        }
        //    }

        //    if (topicsNotIN?.Length > 0)
        //    {
        //        stringBuilder.Append(" &&");
        //        stringBuilder.Append(" !(");

        //        i = 0;

        //        while (true)
        //        {
        //            stringBuilder.Append("'").Append(topicsNotIN[i]).Append("'").Append(" in topics");

        //            i++;
        //            if (i < topicsNotIN.Length)
        //            {
        //                stringBuilder.Append(" || ");
        //            }
        //            else
        //            {
        //                stringBuilder.Append(")");
        //                break;
        //            }
        //        }
        //    }

        //    return stringBuilder.ToString();
        //}


    }
}
