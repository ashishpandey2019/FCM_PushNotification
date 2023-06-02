using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushNotificationHelper.Models;

namespace SaConclave.PushNotification.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly PushNotificationService _pushNotificationService;

        public NotificationController(PushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        //[HttpPost(nameof(RegsiterNotification))]
        //public async Task RegsiterNotification(UserTokenModel userTokenModel)
        //{
        //    await _pushNotificationService.RegisterTopicsForTokenAsync(userTokenModel.Token, userTokenModel.Topics);
        //}

        //[HttpPost(nameof(UnRegsiterNotification))]
        //public async Task UnRegsiterNotification(UserTokenModel userTokenModel)
        //{
        //    await _pushNotificationService.UnRegisterTopicsForTokenAsync(userTokenModel.Token, userTokenModel.Topics);
        //}

        [HttpPost(nameof(SendNotificationAsync))]
        public async Task<ActionResult<IList<PushNotificationSendResult>>> SendNotificationAsync(PushNotificationMessage pushNotificationMessage)
        {
            return Ok(await _pushNotificationService.SendNotificationAsync(pushNotificationMessage));
        }
    }
}
