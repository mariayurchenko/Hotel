using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using System;

namespace SB.Actions.Messages
{
    internal class UpdateBookingHistory : IActionTracking
    {
        private readonly IOrganizationService _service;

        public UpdateBookingHistory(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, out string response)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                throw new Exception($"{nameof(parameters)} is null or white-space");
            }

            if (!Guid.TryParse(parameters, out var id))
            {
                throw new Exception($"{nameof(parameters)} is not parse to Guid");
            }

            var apartment = new Apartment(_service, id);

            apartment.SetBookingFields();
            apartment.Save();

            response = string.Empty;
        }
    }
}