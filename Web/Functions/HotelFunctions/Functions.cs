using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SB.SharedModels.Actions;
using SB.WebShared.DynamicsAuthentication;
using SB.WebShared.Interactors;
using System;
using System.Threading.Tasks;

namespace HotelFunctions
{
    public class Functions
    {
        private readonly IDynamicsInteractor _dynamicsInteractor;

        private const string Cron = "%CRON_EXPRESSION%";

        public Functions(IDynamicsInteractor dynamicsInteractor)
        {
            _dynamicsInteractor = dynamicsInteractor ?? throw new ArgumentNullException(nameof(IAuthenticationService));
        }

        [FunctionName("CloseOldBookings")]
        public async Task CloseOldBookings([TimerTrigger(Cron)] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.CloseOldBookings);

                log.LogInformation("CloseOldBookings success finished");
            }
            catch (Exception e)
            {
                log.LogInformation($"CloseOldBookings finished with error: {e.Message}");
            }
        }

        [FunctionName("SendReminders")]
        public async Task SendReminders([TimerTrigger(Cron)] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.SendReminders);

                log.LogInformation("SendReminders success finished");
            }
            catch (Exception e)
            {
                log.LogInformation($"SendReminders finished with error: {e.Message}");
            }
        }
    }
}