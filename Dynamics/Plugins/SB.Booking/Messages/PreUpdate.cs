using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;
using System;

namespace SB.Booking.Messages
{
    public class PreUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];
                var preImage = context.PreEntityImages["PreImage"];

                var fullBooking = new Shared.EntityProviders.Booking(preImage.Merge(target), service);

                if (Equals(fullBooking.Type, new OptionSetValue(BookingModel.TypeEnum.Отмененный)))
                {
                    target[BookingModel.Fields.Status] = new OptionSetValue(BookingModel.StatusEnum.Inactive);

                    return;
                }

                if (fullBooking.Client == null)
                {
                    target[BookingModel.Fields.Client] = fullBooking.FindOrCreateContact();
                }

                if (string.IsNullOrWhiteSpace(fullBooking.PhoneNumber))
                {
                    target[BookingModel.Fields.PhoneNumber] = fullBooking.GetPhoneNumber();
                }

                if (string.IsNullOrWhiteSpace(fullBooking.ClientName))
                {
                    target[BookingModel.Fields.ClientName] = fullBooking.GetFullName();
                }

                if (string.IsNullOrWhiteSpace(fullBooking.EmailAddress))
                {
                    target[BookingModel.Fields.EmailAddress] = fullBooking.GetEmail();
                }

                if (fullBooking.DateStart == null)
                {
                    throw new ArgumentNullException(nameof(fullBooking.DateStart));
                }

                if (fullBooking.DateEnd == null)
                {
                    throw new ArgumentNullException(nameof(fullBooking.DateEnd));
                }

                if (fullBooking.DateStart.Value > fullBooking.DateEnd.Value)
                {
                    throw new Exception($"{nameof(fullBooking.DateStart)} should be less then {nameof(fullBooking.DateEnd)}");
                }

                if (fullBooking.Apartment == null && fullBooking.ApartmentType != null)
                {
                    target[BookingModel.Fields.Apartment] = Apartment.FindFreeApartment(fullBooking.ApartmentType.Id, fullBooking.DateStart.Value, fullBooking.DateEnd.Value, service);
                }

                if (fullBooking.Apartment != null)
                {
                    var apartment = new Apartment(service, fullBooking.Apartment.Id);

                    if (!apartment.IsApartmentFree(fullBooking.DateStart.Value, fullBooking.DateEnd.Value, fullBooking.Id))
                    {
                        throw new Exception("Apartment is busy");
                    }

                    target[BookingModel.Fields.ApartmentType] = fullBooking.GetApartmentType();
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}