using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using SB.Shared.Models.Dynamics;
using System;
using System.Linq;

namespace SB.Actions.Messages
{
    internal class SendReminders : IActionTracking
    {
        private readonly IOrganizationService _service;
        private readonly IOrganizationService _adminService;

        public SendReminders(IOrganizationService service, IOrganizationService adminService)
        {
            _service = service;
            _adminService = adminService;
        }

        public void Execute(string parameters, out string response)
        {
            var settings = SBCustomSettings.GetSettings(_adminService, SBCustomSettingsModel.Fields.Mailforreceivingletters, SBCustomSettingsModel.Fields.Userforsendingletters, SBCustomSettingsModel.Fields.AppId, SBCustomSettingsModel.Fields.DynamicsUrl);

            if (string.IsNullOrWhiteSpace(settings.Mailforreceivingletters))
            {
                throw new Exception($"{nameof(settings.Mailforreceivingletters)} is null or white-space");
            }
            if (settings.Userforsendingletters == null)
            {
                throw new Exception($"{nameof(settings.Mailforreceivingletters)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(settings.AppId))
            {
                throw new Exception($"{nameof(settings.AppId)} is null or white-space");
            }
            if (settings.DynamicsUrl == null)
            {
                throw new Exception($"{nameof(settings.DynamicsUrl)} is null or white-space");
            }

            var contact = Contact.FindContactByEmail(_service, settings.Mailforreceivingletters);

            if (contact == null)
            {
                contact = new Contact(_service)
                {
                    FirstName = "Mail for receiving letters",
                    Email = settings.Mailforreceivingletters
                };

                contact.Save();
            }

            if (contact.Id == null)
            {
                throw new Exception($"{nameof(contact.Id)} is null");
            }

            var arrivalBookings = Booking.GetTomorrowsArrivalBookings(_service);
            var leavingBookings = Booking.GetTomorrowsLeavingBookings(_service);

            if (leavingBookings.Any() || arrivalBookings.Any())
            {
                var fromParty = new Entity(ActivityPartyModel.LogicalName)
                {
                    [ActivityPartyModel.Fields.Party] = settings.Userforsendingletters
                };

                var toParty = new Entity(ActivityPartyModel.LogicalName)
                {
                    [ActivityPartyModel.Fields.Party] = contact.GetReference()
                };

                if (arrivalBookings.Any())
                {
                    var xml = Email.GetVacationersComeTemplate();

                    foreach (var booking in arrivalBookings)
                    {
                        if (xml.DocumentElement == null)
                        {
                            throw new Exception($"{nameof(xml.DocumentElement)} is null");
                        }

                        var refElem = xml.DocumentElement.LastChild;

                        xml.DocumentElement.InsertAfter(Email.GetHrefXmlElement(xml, booking.ID, booking.GenerateCrmRecordUrl(settings.DynamicsUrl, settings.AppId)), refElem);
                    }

                    var email = new Email(_service)
                    {
                        To = new EntityCollection { Entities = { toParty } },
                        From = new EntityCollection { Entities = { fromParty } },
                        Direction = true,
                        Regarding = contact.GetReference(),
                        Subject = " Отдыхающие приезжают",
                        Description = xml.InnerXml
                    };

                    email.SendEmail();
                }

                if (leavingBookings.Any())
                {
                    var xml = Email.GetVacationersLeaveTemplate();

                    foreach (var booking in leavingBookings)
                    {
                        if (xml.DocumentElement == null)
                        {
                            throw new Exception($"{nameof(xml.DocumentElement)} is null");
                        }

                        var refElem = xml.DocumentElement.LastChild;

                        xml.DocumentElement.InsertAfter(Email.GetHrefXmlElement(xml, booking.ID, booking.GenerateCrmRecordUrl(settings.DynamicsUrl, settings.AppId)), refElem);
                    }

                    var email = new Email(_service)
                    {
                        To = new EntityCollection { Entities = { toParty } },
                        From = new EntityCollection { Entities = { fromParty } },
                        Direction = true,
                        Regarding = contact.GetReference(),
                        Subject = " Отдыхающие уезжают",
                        Description = xml.InnerXml
                    };

                    email.SendEmail();
                }
            }

            response = string.Empty;
        }
    }
}