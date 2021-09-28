using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;
using SB.SharedModels.Booking;
using System;

namespace SB.Actions.Messages
{
    public class CreateBooking : IActionTracking
    {
        private readonly IOrganizationService _service;
        private readonly IOrganizationService _adminService;

        public CreateBooking(IOrganizationService service, IOrganizationService adminService)
        {
            _service = service;
            _adminService = adminService;
        }

        public void Execute(string parameters, out string response)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                throw new Exception($"{nameof(parameters)} is null or white-space");
            }

            var request = JsonSerializer.Deserialize<CreateBookingRequest>(parameters);

            if (request == null)
            {
                throw new Exception($"{nameof(request)} is null");
            }
            if (request.ApartmentId == Guid.Empty)
            {
                throw new Exception($"{nameof(request.ApartmentId)} is empty Guid");
            }
            if (request.Adults <= 0)
            {
                throw new Exception($"{nameof(request.Adults)} must be more than 0");
            }
            if (request.Children != null && request.Children < 0)
            {
                throw new Exception($"{nameof(request.Adults)} must be more or equal 0");
            }
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                throw new Exception($"{nameof(request.PhoneNumber)} is null or white-space");
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

            var time = new TimeSpan(0, 12, 0, 0);

            dateEnd = dateEnd.Date + time;
            dateStart = dateStart.Date + time;

            var settings = SBCustomSettings.GetSettings(_adminService, SBCustomSettingsModel.Fields.Userforsendingletters);

            if (settings.Userforsendingletters == null)
            {
                throw new Exception($"{nameof(settings.Mailforreceivingletters)} is null or white-space");
            }

            var apartmentType = new ApartmentType(_service, request.ApartmentId);

            var booking = new Booking(_service)
            {
                Adults = request.Adults,
                Childrens = request.Children,
                ApartmentType = apartmentType.GetReference(),
                Apartment = Apartment.FindFreeApartment(request.ApartmentId, dateStart, dateEnd, _service),
                DateStart = dateStart,
                DateEnd = dateEnd,
                ClientName = request.ClientName,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.Email,
                Description = request.Description,
                Price = request.Price,
                Type = new OptionSetValue(BookingModel.TypeEnum.Новый)
            };

            booking.Save();

            var id = booking.Id;

            if (id == null)
            {
                throw new Exception("Booking is not create");
            }

            booking = new Booking(id.Value, new ColumnSet(BookingModel.Fields.ID, BookingModel.Fields.Client), _service);

            var responseModel = new CreateBookingResponse
            {
                BookingId = id.Value,
                BookingNumber = booking.ID
            };

            response = JsonSerializer.Serialize(responseModel);
        }
    }
}