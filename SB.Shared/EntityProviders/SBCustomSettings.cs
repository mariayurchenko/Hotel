using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;
using System;
using System.Linq;
using System.Text;

namespace SB.Shared.EntityProviders
{
    public class SBCustomSettings : SBCustomSettingsModel
    {
        public SBCustomSettings(IOrganizationService service) : base(service)
        {
        }

        public SBCustomSettings(IOrganizationService service, Guid id) : base(id, service)
        {
        }

        public SBCustomSettings(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }

        public SBCustomSettings(Entity entity, IOrganizationService service) : base(entity, service)
        {
        }

        public static SBCustomSettings GetSettings(IOrganizationService service, params string[] columns)
        {
            var query = new QueryExpression(LogicalName)
            {
                ColumnSet = new ColumnSet(columns)
            };
            var settings = service.RetrieveMultiple(query).Entities.Select(entity => new SBCustomSettings(entity, service)).FirstOrDefault();

            if (settings == null)
            {
                throw new InvalidOperationException("SB Custom settings not found. Please configure system or contact the system administrator for support.");
            }

            return settings;
        }

        private SBCustomSettings GetDirectionSettings(string directionName)
        {
            var query = new QueryExpression(LogicalName)
            {
                ColumnSet = new ColumnSet(Fields.All.Where(s => s.StartsWith(directionName)).ToArray())
            };

            var settings = _service.RetrieveMultiple(query).Entities.FirstOrDefault();

            return settings == null ? null : new SBCustomSettings(settings, _service);
        }

        public static string GetBasicHeader(string userName, string userPassword)
        {
            return "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + userPassword));
        }
    }
}