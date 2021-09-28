using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;
using System;
using System.Linq;

namespace SB.Shared.EntityProviders
{
    public class Price : PriceModel
    {
        public Price(IOrganizationService service) : base(service) { }
        public Price(IOrganizationService service, Guid id) : base(id, service) { }
        public Price(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Price(Entity entity, IOrganizationService service) : base(entity, service) { }

        public static string CreateName(string apartment)
        {
            return $"{apartment} от {DateTime.Now:dd.MM.yyyy}";
        }

        public static int GetPriceByDate(Guid apartmentTypeId, DateTime date, IOrganizationService service)
        {
            if (apartmentTypeId == Guid.Empty)
            {
                throw new Exception($"{nameof(apartmentTypeId)} is empty Guid");
            }

            var query = new QueryExpression(LogicalName);

            query.Orders.Add(new OrderExpression(Fields.DateStart, OrderType.Descending));
            query.ColumnSet.AddColumns(Fields.PriceValue, Fields.Name);
            query.Criteria.AddCondition(Fields.DateStart, ConditionOperator.OnOrBefore, date);
            query.Criteria.AddCondition(Fields.ApartmentType, ConditionOperator.Equal, apartmentTypeId);

            var price = service.RetrieveMultiple(query).ToEntityCollection<Price>(service).FirstOrDefault();

            if (price == null)
            {
                throw new Exception($"Price not found");
            }

            if (price.PriceValue == null)
            {
                throw new Exception($"Price Value in {price.Name} is null");
            }

            if (price.PriceValue <= 0)
            {
                throw new Exception("Price less or equal 0");
            }

            return price.PriceValue.Value;
        }

        public static int GetPriceByDate(Guid apartmentTypeId, DateTime dateStart, DateTime dateEnd, IOrganizationService service)
        {
            var price = 0;

            do
            {
                price += GetPriceByDate(apartmentTypeId, dateStart, service);
                dateStart = dateStart.AddDays(1);
            } while (dateStart.Day != dateEnd.Day || dateStart.Month != dateEnd.Month || dateStart.Year != dateEnd.Year);

            return price;
        }

        public static int? GetPriceByDateOrReturnNull(Guid apartmentTypeId, DateTime dateStart, DateTime dateEnd, IOrganizationService service)
        {
            try
            {
                return GetPriceByDate(apartmentTypeId, dateStart, dateEnd, service);
            }
            catch
            {
                return null;
            }
        }

    }
}