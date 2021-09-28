using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.Shared.EntityProviders;
using System;
using System.Linq;
using SB.SharedModels.Apartments;
using ApartmentTypeModel = SB.Shared.Models.Dynamics.ApartmentTypeModel;

namespace SB.Actions.Messages
{
    internal class GetAvailableApartments : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetAvailableApartments(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, out string response)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                throw new Exception($"{nameof(parameters)} is null or white-space");
            }

            var request = JsonSerializer.Deserialize<GetAvailableApartmentsRequest>(parameters);

            if (request == null)
            {
                throw new Exception($"{nameof(request)} is null");
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

            var apartmentTypeList = ApartmentType.GetAvailableApartmentTypes(dateStart, dateEnd, _service, ApartmentTypeModel.Fields.MainImage, ApartmentTypeModel.Fields.Name, ApartmentTypeModel.Fields.Description);

            var responseModel = new GetAvailableApartmentsResponse
            {
                ApartmentTypes = apartmentTypeList.Where(type => type.Id.HasValue)
                    .Select(type => new SharedModels.Apartments.ApartmentTypeModel
                    {
                        Id = type.Id.Value,
                        Price = Price.GetPriceByDateOrReturnNull(type.Id.Value, dateStart, dateEnd, _service),
                        MainImage = type.GetMainImage(),
                        Title = type.Name
                    })
                    .ToList()
            };

            response = JsonSerializer.Serialize(responseModel);
        }
    }
}