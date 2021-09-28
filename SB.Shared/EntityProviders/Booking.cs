using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SB.Shared.EntityProviders
{
    public class Booking : BookingModel
    {
        public Booking(IOrganizationService service) : base(service) { }
        public Booking(IOrganizationService service, Guid id) : base(id, service) { }
        public Booking(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Booking(Entity entity, IOrganizationService service) : base(entity, service) { }

        public EntityReference FindOrCreateContact()
        {
            Client = FindContactOrReturnNull() ?? CreateContact();

            return Client;
        }

        public static void CloseOldBookings(IOrganizationService service, DateTime date)
        {
            var query = new QueryExpression(LogicalName);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Active);
            query.Criteria.AddCondition(Fields.DateEnd, ConditionOperator.OnOrBefore, date);

            var bookings = service.RetrieveMultiple(query).ToEntityCollection<Booking>(service);

            foreach (var booking in bookings)
            {
                booking.Status = new OptionSetValue(StatusEnum.Inactive);

                booking.Save();
            }
        }

        public static List<Booking> GetTomorrowsLeavingBookings(IOrganizationService service)
        {
            var query = new QueryExpression(LogicalName);
            query.ColumnSet.AddColumn(Fields.ID);
            query.Criteria.AddCondition(Fields.Type, ConditionOperator.NotEqual, TypeEnum.Отмененный);
            query.Criteria.AddCondition(Fields.DateEnd, ConditionOperator.On, DateTime.Now.AddDays(1));

            var bookings = service.RetrieveMultiple(query).ToEntityCollection<Booking>(service);

            return bookings;
        }

        public static List<Booking> GetTomorrowsArrivalBookings(IOrganizationService service)
        {
            var query = new QueryExpression(LogicalName);
            query.ColumnSet.AddColumn(Fields.ID);
            query.Criteria.AddCondition(Fields.Type, ConditionOperator.NotEqual, TypeEnum.Отмененный);
            query.Criteria.AddCondition(Fields.DateStart, ConditionOperator.On, DateTime.Now.AddDays(1));

            var bookings = service.RetrieveMultiple(query).ToEntityCollection<Booking>(service);

            return bookings;
        }

        public string GenerateCrmRecordUrl(string dynamicsUrl, string appId)
        {
            if (string.IsNullOrWhiteSpace(dynamicsUrl))
            {
                throw new Exception($"{nameof(dynamicsUrl)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new Exception($"{nameof(appId)} is null or white-space");
            }
            if (Id == null)
            {
                throw new ArgumentNullException($"{nameof(Id)} is null");
            }

            dynamicsUrl = dynamicsUrl.EndsWith("/") ? dynamicsUrl : dynamicsUrl + "/";

            return $"{dynamicsUrl}main.aspx?appid={appId}&pagetype=entityrecord&etn={LogicalName}&id={Id.Value}";
        }

        public string GetPhoneNumber()
        {
            var contact = GetContact(ContactModel.Fields.MobilePhone);

            return contact.MobilePhone;
        }

        public string GetFullName()
        {
            var contact = GetContact(ContactModel.Fields.FullName);

            return contact.FullName;
        }

        public string GetEmail()
        {
            var contact = GetContact(ContactModel.Fields.Email);

            return contact.Email;
        }

        public EntityReference GetApartmentType()
        {
            if (Apartment == null)
            {
                throw new ArgumentNullException(nameof(Apartment));
            }

            var apartment = new Apartment(Apartment.Id, new ColumnSet(ApartmentModel.Fields.Type), _service);

            return apartment.Type;
        }

        private Contact GetContact(params string[] columns)
        {
            if (Client == null)
            {
                throw new ArgumentNullException(nameof(Client));
            }

            return new Contact(Client.Id, new ColumnSet(columns), _service);
        }

        private EntityReference FindContactOrReturnNull()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                throw new Exception($"{nameof(PhoneNumber)} is null or white-space");
            }

            var query = new QueryExpression(ContactModel.LogicalName);
            query.Criteria.AddCondition(ContactModel.Fields.MobilePhone, ConditionOperator.Equal, PhoneNumber);

            if (!string.IsNullOrWhiteSpace(EmailAddress))
            {
                query.Criteria.AddCondition(ContactModel.Fields.Email, ConditionOperator.Equal, EmailAddress);
            }

            return _service.RetrieveMultiple(query).Entities.FirstOrDefault()?.ToEntityReference();
        }

        private EntityReference CreateContact()
        {
            var contact = new Contact(_service);

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                throw new Exception($"{nameof(PhoneNumber)} is null or white-space");
            }

            contact.FirstName = ClientName;
            contact.MobilePhone = PhoneNumber;
            contact.Email = EmailAddress;

            contact.Save();

            return contact.GetReference();
        }
    }
}