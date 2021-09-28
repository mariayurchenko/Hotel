using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;
using System;

namespace SB.Shared.EntityProviders
{
    public class Image : ImageModel
    {
        public Image(IOrganizationService service) : base(service)
        {
        }

        public Image(IOrganizationService service, Guid id) : base(id, service)
        {
        }

        public Image(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }

        public Image(Entity entity, IOrganizationService service) : base(entity, service)
        {
        }
    }
}