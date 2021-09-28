using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;
using System;

namespace SB.Actions.Messages
{
    internal class CalculatePrice : IActionTracking
    {
        private readonly IOrganizationService _service;

        public CalculatePrice(IOrganizationService service)
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

            var booking = new Booking(id, new ColumnSet(BookingModel.Fields.DateEnd, BookingModel.Fields.DateStart, BookingModel.Fields.ApartmentType), _service);

            if (booking.DateEnd == null)
            {
                throw new Exception($"{nameof(booking.DateEnd)} is null");
            }
            if (booking.DateStart == null)
            {
                throw new Exception($"{nameof(booking.DateStart)} is null");
            }
            if (booking.ApartmentType == null)
            {
                throw new Exception($"{nameof(booking.ApartmentType)} is null");
            }

            var price = Price.GetPriceByDate(booking.ApartmentType.Id, booking.DateStart.Value, booking.DateEnd.Value, _service);
         
            response = price.ToString();
        }
    }
}