
using PushNotificationHelper;
using PushNotificationHelper.Models;


class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            //var result = await PushNotification.SendNotificationAsync(new PushNotification.PushNotificationMessage
            //{
            //    Body = "Testing from 1",
            //    Title = "title",
            //    Data = new Dictionary<string, string> { { "Type", "DetailPage" }, { "Id", "78" } },
            //    UserTypes = new string[] { "Admin","Member", "NonMember" },
            //    NotToSent = new string[] { "8958640544"}
            //},ff);
            var clint = new HttpClient();
            clint.BaseAddress = new Uri("https://localhost:7184/");
            NotificationClient notificationClient = new NotificationClient(clint, ff);


            //await notificationClient.RegsiterNotification(new SIDM.PushNotifcationManager.API.Models.UserTokenModel
            //{
            //    Token = "cz3D7GIoTiQ:APA91bFOwfq5uZZPBSsBLeDde0Y_g62dVl0aa7Tq9hHAvQ4WfrwyNJaiNs-596fX1FfY2gQE9SdChGUzVCdQdd_KP_el6wIU_Yn5Qr3OD7q8C8QfQPSFdZ3vsQ2Y4yGyryy7aszhb7ho",
            //    Topics = new string[] { "9898989898" }
            //});


            var result = await notificationClient.SendNotificationAsync(new PushNotificationMessage
            {
                Title = "Test Notification",
                Body = "this is test message from api",
                Tokens = new string[] { "cpG9CaDlTU-gTLFGKtphYx:APA91bHRTIRePB9MrKC3BrFBlmK9Z0fUyEG8eAC6KbxwzgNGkghk7y6WGoo5mQv7xC8h-43gHoOpwcS7yVHRlIm4_4O3oZNPw_qdTUeKwmBdJyiYAiBdE-vVL_mZ3Jn38GGdAXJH20eo" }
            });
            Console.WriteLine(result);

            //await PushNotification.RegisterTopicsForTokenAsync("cz3D7GIoTiQ:APA91bFOwfq5uZZPBSsBLeDde0Y_g62dVl0aa7Tq9hHAvQ4WfrwyNJaiNs-596fX1FfY2gQE9SdChGUzVCdQdd_KP_el6wIU_Yn5Qr3OD7q8C8QfQPSFdZ3vsQ2Y4yGyryy7aszhb7ho",
            //      ff, "Admin", "9898989898");

            //await PushNotification.UnRegisterTopicsForTokenAsync("fiUfZzED208:APA91bGGRrzSFjiAWSjdivvZMTgjM-vsquv9s_EuaF2bgw5Tw6V-h_mrPG7o3UIUnBaKj8w7nW6zRMB4PkPWxtHrIOQwq3GSazo6-5c95Xwcaa--tlj2adopbjlvTC-cnf5bK_YuXeBK",
            //     ff, "Admin", "8727864609");
            // Console.WriteLine(result);
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee);
        }
        Console.ReadLine();

    }

    private static void ff(string text)
    {
        Console.WriteLine(text);
    }

}