using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using SB.Actions.Messages;
using SB.SharedModels.Actions;
using System;

namespace SB.Actions
{
    public class ActionTracking : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            var factory = serviceProvider.Get<IOrganizationServiceFactory>();
            //var notificationService = serviceProvider.Get<IServiceEndpointNotificationService>();
            var service = factory.CreateOrganizationService(context.UserId);
            var adminService = factory.CreateOrganizationService(null);
            var tracer = serviceProvider.Get<ITracingService>();

            try
            {
                var actionName = (string)context.InputParameters["ActionName"];
                var parameters = context.InputParameters.Contains("Parameters") ? (string)context.InputParameters["Parameters"] : string.Empty;

                string response;

                foreach (var parameter in context.InputParameters)
                {
                    if (parameter.Value != null &&  parameter.Value.ToString().Length < 50)
                    {
                        tracer?.Trace($"{parameter.Key} = {parameter.Value}");
                    }
                }

                switch (actionName)
                {
                    case ActionNames.ActionTrackingNames.GetCurrentPrice:
                        new GetCurrentPrice(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.UpdateBookingHistory:
                        new UpdateBookingHistory(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.CalculatePrice:
                        new CalculatePrice(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.CloseOldBookings:
                        new CloseOldBookings(service, adminService).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.SendReminders:
                        new SendReminders(service, adminService).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.CreateBooking:
                        new CreateBooking(service, adminService).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.GetAvailableApartments:
                        new GetAvailableApartments(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.GetApartment:
                        new GetApartment(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.GetAllApartments:
                        new GetAllApartments(service).Execute(parameters, out response);
                        break;

                    case ActionNames.ActionTrackingNames.GetPriceIfApartmentTypeAvailable:
                        new GetPriceIfApartmentTypeAvailable(service).Execute(parameters, out response);
                        break;

                    default:
                        throw new Exception($"Acton message with name {actionName} wasn't found in {nameof(ActionTracking)}");
                }

                context.OutputParameters["Response"] = response;

                tracer?.Trace($"{nameof(context.OutputParameters)}:");
                foreach (var parameter in context.OutputParameters)
                {
                    if (parameter.Value != null && parameter.Value.ToString().Length < 50)
                    {
                        tracer?.Trace($"{parameter.Key} = {parameter.Value}");
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}