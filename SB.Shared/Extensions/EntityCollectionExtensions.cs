using Microsoft.Xrm.Sdk;
using SB.Shared.Models.Dynamics;
using System.Collections.Generic;
using System.Linq;

namespace SB.Shared.Extensions
{
    public static class EntityCollectionExtensions
    {
        public static List<T> ToEntityCollection<T>(this EntityCollection collection, IOrganizationService service) where T : EntityBase
        {
            var list = collection.Entities.ToList();

            return list.ConvertAll(x => x.ToEntity<T>(service));
        }
    }
}