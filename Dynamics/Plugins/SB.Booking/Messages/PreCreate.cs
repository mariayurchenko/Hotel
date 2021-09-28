using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;
using System;

namespace SB.Booking.Messages
{
    public class PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var booking = new Shared.EntityProviders.Booking(target, service);

                if (booking.Type == null)
                {
                    target[BookingModel.Fields.Type] = new OptionSetValue(BookingModel.TypeEnum.Новый);
                }

                if (booking.Client == null)
                {
                    target[BookingModel.Fields.Client] = booking.FindOrCreateContact();
                }

                if (string.IsNullOrWhiteSpace(booking.PhoneNumber))
                {
                    target[BookingModel.Fields.PhoneNumber] = booking.GetPhoneNumber();
                }

                if (string.IsNullOrWhiteSpace(booking.ClientName))
                {
                    target[BookingModel.Fields.ClientName] = booking.GetFullName();
                }

                if (string.IsNullOrWhiteSpace(booking.EmailAddress))
                {
                    target[BookingModel.Fields.EmailAddress] = booking.GetEmail();
                }

                if (booking.DateStart == null)
                {
                    throw new ArgumentNullException(nameof(booking.DateStart));
                }

                if (booking.DateEnd == null)
                {
                    throw new ArgumentNullException(nameof(booking.DateEnd));
                }

                if (booking.DateStart.Value > booking.DateEnd.Value)
                {
                    throw new Exception($"{nameof(booking.DateStart)} should be less then {nameof(booking.DateEnd)}");
                }

                if (booking.Apartment == null && booking.ApartmentType != null)
                {
                    target[BookingModel.Fields.Apartment] = Apartment.FindFreeApartment(booking.ApartmentType.Id, booking.DateStart.Value, booking.DateEnd.Value, service);
                }

                if (booking.Apartment != null)
                {
                    var apartment = new Apartment(service, booking.Apartment.Id);

                    if (!apartment.IsApartmentFree(booking.DateStart.Value, booking.DateEnd.Value))
                    {
                        throw new Exception("Apartment is busy");
                    }

                    target[BookingModel.Fields.ApartmentType] = booking.GetApartmentType();
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}