using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;
using System;
using System.Linq;

namespace SB.Shared.EntityProviders
{
    public class Contact : ContactModel
    {
        public Contact(IOrganizationService service) : base(service)
        {
        }

        public Contact(IOrganizationService service, Guid id) : base(id, service)
        {
        }

        public Contact(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }

        public Contact(Entity entity, IOrganizationService service) : base(entity, service)
        {
        }

        public static Contact FindContactByEmail(IOrganizationService service, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception($"{nameof(email)} is null or white-space");
            }

            var query = new QueryExpression(LogicalName);
            query.Criteria.AddCondition(Fields.Email, ConditionOperator.Equal, email);

            var contact = service.RetrieveMultiple(query).ToEntityCollection<Contact>(service).FirstOrDefault();

            return contact;
        }
    }
}