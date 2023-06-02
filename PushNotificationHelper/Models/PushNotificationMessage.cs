using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationHelper.Models
{
    public class PushNotificationMessage
    {
        public IReadOnlyList<string> Tokens { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IReadOnlyDictionary<string, string>? Data { get; set; }
    }


    public class PushNotificationSendResult
    {
        public string Token { get; set; }
        public string Result { get; set; }
    }
}
