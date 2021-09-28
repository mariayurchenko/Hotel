using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.SharedModels.Apartments;
using System;
using Apartment = SB.Shared.EntityProviders.Apartment;

namespace SB.Actions.Messages
{
    internal class GetPriceIfApartmentTypeAvailable : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetPriceIfApartmentTypeAvailable(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, out string response)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                throw new Exception($"{nameof(parameters)} is null or white-space");
            }

            var request = JsonSerializer.Deserialize<GetPriceIfApartmentTypeAvailableRequest>(parameters);

            if (request == null)
            {
                throw new Exception($"{nameof(request)} is null");
            }
            if (request.ApartmentTypeId == Guid.Empty)
            {
                throw new Exception($"{nameof(request.ApartmentTypeId)} is empty Guid");
            }
            if (string.IsNullOrWhiteSpace(request.DateStart))
            {
                throw new Exception($"{nameof(request.DateStart)} null or empty white-space");
            }
            if (string.IsNullOrWhiteSpace(request.DateEnd))
            {
                throw new Exception($"{nameof(request.DateEnd)} null or empty white-space");
            }

            DateTime dateEnd;

            if (DateTime.TryParse(request.DateStart, out var dateStart))
            {
                if (DateTime.TryParse(request.DateEnd, out dateEnd))
                {
                    if (dateStart.Date < DateTime.Today.Date)
                    {
                        throw new Exception($"{nameof(request.DateStart)} should be future");
                    }
                    if (dateStart.Date > dateEnd.Date)
                    {
                        throw new Exception($"{nameof(request.DateStart)} should be less then {nameof(request.DateEnd)}");
                    }
                }
                else
                {
                    throw new Exception($"{nameof(request.DateEnd)} not parse to DateTime");
                }
            }
            else
            {
                throw new Exception($"{nameof(request.DateStart)} not parse to DateTime");
            }

            var apartment = Apartment.FindFreeApartment(request.ApartmentTypeId, dateStart, dateEnd, _service);

            if (apartment == null)
            {
                throw new Exception("Free apartments not found");
            }

            var price = Price.GetPriceByDateOrReturnNull(request.ApartmentTypeId, dateStart, dateEnd, _service);

            response = price.ToString();
        }
    }
}