using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.SharedModels.Apartments;
using System.Linq;
using ApartmentTypeModel = SB.Shared.Models.Dynamics.ApartmentTypeModel;

namespace SB.Actions.Messages
{
    public class GetAllApartments : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetAllApartments(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, out string response)
        {
            var apartmentTypeList = ApartmentType.GetAllActiveApartmentTypes(_service, ApartmentTypeModel.Fields.MainImage, ApartmentTypeModel.Fields.Name, ApartmentTypeModel.Fields.Description);

            var responseModel = new GetAvailableApartmentsResponse
            {
                ApartmentTypes = apartmentTypeList.Where(type => type.Id.HasValue)
                    .Select(type => new SharedModels.Apartments.ApartmentTypeModel
                    {
                        Id = type.Id.Value,
                        MainImage = type.GetMainImage(),
                        Title = type.Name
                    })
                    .ToList()
            };

            response = JsonSerializer.Serialize(responseModel);
        }
    }
}