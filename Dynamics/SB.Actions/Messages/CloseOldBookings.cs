using System;
using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;

namespace SB.Actions.Messages
{
    internal class CloseOldBookings : IActionTracking
    {
        private readonly IOrganizationService _service;
        private readonly IOrganizationService _adminService;

        public CloseOldBookings(IOrganizationService service, IOrganizationService adminService)
        {
            _service = service;
            _adminService = adminService;
        }

        public void Execute(string parameters, out string response)
        {
            var settings = SBCustomSettings.GetSettings(_adminService, SBCustomSettingsModel.Fields.CloseBookingFromDays);

            if (settings.CloseBookingFromDays == null)
            {
                throw new Exception($"{nameof(settings.CloseBookingFromDays)} is null");
            }
            
            var now = DateTime.Now;

            var date = now - (now.AddDays(settings.CloseBookingFromDays.Value) - now);

            Booking.CloseOldBookings(_service, date);

            response = string.Empty;
        }
    }
}