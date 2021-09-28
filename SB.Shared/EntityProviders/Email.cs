using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;
using System;
using System.Xml;
using Microsoft.Crm.Sdk.Messages;

namespace SB.Shared.EntityProviders
{
    public class Email : EmailModel
    {
        public Email(IOrganizationService service) : base(service) { }
        public Email(IOrganizationService service, Guid id) : base(id, service) { }
        public Email(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Email(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static XmlDocument GetVacationersComeTemplate()
        {
            return GetBookingTemplate("Завтра приезжают отдыхающие!");
        }

        public static XmlDocument GetVacationersLeaveTemplate()
        {
            return GetBookingTemplate("Завтра уезжают отдыхающие!");
        }

        public static XmlElement GetHrefXmlElement(XmlDocument xml, string text, string url)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception($"{nameof(text)} is null or white-space");
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception($"{nameof(url)} is null or white-space");
            }
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new Exception($"{nameof(url)} not parse to uri");
            }

            var href = xml.CreateElement("div");
            href.SetAttribute("style", "font-size:12pt");

            var a = xml.CreateElement("a");
            a.SetAttribute("href", url);
            a.InnerText = text;

            href.AppendChild(a);

            return href;
        }

        public static XmlElement GetNewLineXmlElement(XmlDocument xml)
        {
            var element = xml.CreateElement("br");

            return element;
        }

        public void SendEmail()
        {
            if (Id == null)
            {
                Save();
            }

            if (Id == null)
            {
                throw new Exception($"{nameof(Id)} is null");
            }

            var sendEmailRequest = new SendEmailRequest
            {
                EmailId = Id.Value,
                IssueSend = true
            };

            _service.Execute(sendEmailRequest);
        }

        private static XmlDocument GetBookingTemplate(string headerText)
        {
            var xml = new XmlDocument();
            xml.AppendChild(xml.CreateElement("div"));

            var header = xml.CreateElement("div");
            header.SetAttribute("style", "font-size:16pt");

            var strong = xml.CreateElement("strong");
            strong.InnerText = headerText;

            header.AppendChild(strong);

            var booking = xml.CreateElement("div");
            booking.SetAttribute("style", "font-size:14pt");
            booking.InnerText = "Бронирования:";

            if (xml.DocumentElement == null)
            {
                throw new Exception($"{nameof(xml.DocumentElement)} is null");
            }

            var refElem = xml.DocumentElement.LastChild;

            xml.DocumentElement.InsertAfter(GetNewLineXmlElement(xml), refElem);
            xml.DocumentElement.InsertAfter(booking, refElem);
            xml.DocumentElement.InsertAfter(GetNewLineXmlElement(xml), refElem);
            xml.DocumentElement.InsertAfter(header,refElem);

            return xml;
        }
    }
}