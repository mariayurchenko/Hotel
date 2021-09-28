using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;
using System;

namespace SB.Actions.Messages
{
    public class GetApartment : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetApartment(IOrganizationService service)
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
                throw new Exception($"{nameof(parameters)} is not try parse to Guid");
            }

            Entity entity;

            try
            {
                entity = _service.Retrieve(ApartmentTypeModel.LogicalName, id, new ColumnSet(ApartmentTypeModel.Fields.Name, ApartmentTypeModel.Fields.Description, ApartmentTypeModel.Fields.MainImage));
            }
            catch
            {
                response = null;

                return;
            }

            var apartment = new ApartmentType(entity, _service);

            var responseModel = new SharedModels.Apartments.Apartment
            {
                Id = id,
                Description = apartment.Description,
                MainImage = apartment.GetMainImage(),
                Title = apartment.Name,
                Images = apartment.GetAllImages()
            };

            response = JsonSerializer.Serialize(responseModel);
        }
    }
}