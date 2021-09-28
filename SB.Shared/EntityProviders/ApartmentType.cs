using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Models.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using SB.Shared.Extensions;

namespace SB.Shared.EntityProviders
{
    public class ApartmentType : ApartmentTypeModel
    {
        public ApartmentType(IOrganizationService service) : base(service)
        {
        }

        public ApartmentType(IOrganizationService service, Guid id) : base(id, service)
        {
        }

        public ApartmentType(Guid id, ColumnSet columnSet, IOrganizationService service)
            : base(service.Retrieve(LogicalName, id, columnSet), service)
        {
        }

        public ApartmentType(Entity entity, IOrganizationService service) : base(entity, service)
        {
        }

        public static List<ApartmentType> GetAvailableApartmentTypes(DateTime start, DateTime end, IOrganizationService service, params string[] columns)
        {
            var query = new QueryExpression(LogicalName);
            query.ColumnSet.AddColumns(columns);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Active);

            var link = query.AddLink(ApartmentModel.LogicalName, Fields.PrimaryId, ApartmentModel.Fields.Type);
            link.Columns.AddColumns(ApartmentModel.Fields.PrimaryId);
            link.LinkCriteria.AddCondition(ApartmentModel.Fields.Status, ConditionOperator.Equal, ApartmentModel.StatusEnum.Active);
            var entityCollection = service.RetrieveMultiple(query);

            for (var index = 0; index < entityCollection.Entities.Count; index++)
            {
                var apartment = new Apartment(service, new Guid(entityCollection.Entities[index].GetAttributeValue<AliasedValue>("sb_apartment1.sb_apartmentid").Value.ToString()));

                if (apartment.IsApartmentFree(start, end)) continue;

                entityCollection.Entities.Remove(entityCollection.Entities[index]);
                index--;
            }

            var list = new List<ApartmentType>();
            var apartmentType = entityCollection.ToEntityCollection<ApartmentType>(service);

            var typeId = apartmentType.Select(a => a.Id).ToList().Distinct().ToArray();

            foreach (var id in typeId)
            {
                list.Add(apartmentType.FirstOrDefault(a => a.Id == id));
            }

            return list;
        }

        public static List<ApartmentType> GetAllActiveApartmentTypes(IOrganizationService service, params string[] columns)
        {
            var query = new QueryExpression(LogicalName);
            query.ColumnSet.AddColumns(columns);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Active);

            return service.RetrieveMultiple(query).ToEntityCollection<ApartmentType>(service);
        }

        public List<string> GetAllImages()
        {
            if (!Id.HasValue)
            {
                throw new Exception($"{nameof(Id)} is null");
            }

            var query = new QueryExpression(ApartmentModel.LogicalName);
            query.Criteria.AddCondition(ApartmentModel.Fields.Status, ConditionOperator.Equal, StatusEnum.Active);
            query.Criteria.AddCondition(ApartmentModel.Fields.Type, ConditionOperator.Equal, Id.Value);

            var link = query.AddLink(ImageModel.LogicalName, ApartmentModel.Fields.PrimaryId, ImageModel.Fields.Apartment);
            link.Columns.AddColumn("sb_photo");
            link.LinkCriteria.AddCondition(ImageModel.Fields.Status, ConditionOperator.Equal, ImageModel.StatusEnum.Active);

            var images = _service.RetrieveMultiple(query).Entities;

            var list = new List<string>();

            foreach (var image in images)
            {
                var bytes = (byte[])image.GetAttributeValue<AliasedValue>("sb_image1.sb_photo").Value;
                list.Add(Convert.ToBase64String(bytes, 0, bytes.Length));
            }

            return list;
        }

        public string GetMainImage()
        {
            if (MainImage == null) return null;

            var image = _service.Retrieve(ImageModel.LogicalName, MainImage.Id, new ColumnSet("sb_photo"));

            var bytes =  image.GetAttributeValue<byte[]>("sb_photo");

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
    }
}