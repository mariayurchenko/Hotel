using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SB.Shared.Extensions;
using SB.Shared.Models.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace SB.Shared.EntityProviders
{
    public class Apartment : ApartmentModel
    {
        public Apartment(IOrganizationService service) : base(service) { }
        public Apartment(IOrganizationService service, Guid id) : base(id, service) { }
        public Apartment(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Apartment(Entity entity, IOrganizationService service) : base(entity, service) { }

        public void SetBookingFields()
        {
            if (Id == null || Id.Value == Guid.Empty)
            {
                throw new Exception("Id is null or empty Guid");
            }

            var now = DateTime.Now;

            var query = new QueryExpression(BookingModel.LogicalName);

            query.Criteria.AddCondition(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id.Value);
            query.Criteria.AddCondition(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный);
            query.Criteria.AddCondition(BookingModel.Fields.Status, ConditionOperator.Equal, BookingModel.StatusEnum.Active);
            query.Criteria.AddCondition(BookingModel.Fields.DateEnd, ConditionOperator.OnOrAfter, now);
            query.Criteria.AddCondition(BookingModel.Fields.DateStart, ConditionOperator.OnOrBefore, now);

            CurrentBooking = _service.RetrieveMultiple(query).Entities.FirstOrDefault()?.ToEntityReference();

            query = new QueryExpression(BookingModel.LogicalName);

            query.ColumnSet.AddColumn(BookingModel.Fields.DateStart);
            query.Criteria.AddCondition(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id.Value);
            query.Criteria.AddCondition(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный);
            query.Criteria.AddCondition(BookingModel.Fields.Status, ConditionOperator.Equal, BookingModel.StatusEnum.Active);
            query.Criteria.AddCondition(BookingModel.Fields.DateStart, ConditionOperator.GreaterThan, now);

            var nextBookings = _service.RetrieveMultiple(query).ToEntityCollection<Booking>(_service);

            if (nextBookings != null && nextBookings.Count > 0)
            {
                var next = nextBookings[0];

                for (var index = 1; index < nextBookings.Count; index++)
                {
                    if (nextBookings[index].DateStart < next.DateStart)
                    {
                        next = nextBookings[index];
                    }
                }

                NextBooking = next.GetReference();
            }
            else
                NextBooking = null;

            query = new QueryExpression(BookingModel.LogicalName);

            query.ColumnSet.AddColumn(BookingModel.Fields.DateEnd);
            query.Criteria.AddCondition(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id.Value);
            query.Criteria.AddCondition(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный);
            query.Criteria.AddCondition(BookingModel.Fields.DateEnd, ConditionOperator.LessThan, now);

            var lastBookings = _service.RetrieveMultiple(query).ToEntityCollection<Booking>(_service);

            if (lastBookings != null && lastBookings.Count > 0)
            {
                var last = lastBookings[0];

                for (var index = 1; index < lastBookings.Count; index++)
                {
                    if (lastBookings[index].DateEnd > last.DateEnd)
                    {
                        last = lastBookings[index];
                    }
                }

                LastBooking = last.GetReference();
            }
            else
                LastBooking = null;
        }
        public static EntityReference FindFreeApartment(Guid apartmentTypeId, DateTime dateStart, DateTime dateEnd, IOrganizationService service)
        {
            if (apartmentTypeId == Guid.Empty)
            {
                throw new Exception($"{nameof(apartmentTypeId)} is empty Guid");
            }

            var type = new ApartmentType(service, apartmentTypeId);

            if (dateStart > dateEnd)
            {
                throw new Exception($"{nameof(dateStart)} should be less then {nameof(dateEnd)}");
            }

            var query = new QueryExpression(LogicalName);
            query.Criteria.AddCondition(Fields.Type, ConditionOperator.Equal, type.Id);
            query.Criteria.AddCondition(Fields.Status, ConditionOperator.Equal, StatusEnum.Active);

            var apartments = service.RetrieveMultiple(query).ToEntityCollection<Apartment>(service);

            if (!apartments.Any())
            {
                throw new Exception("Active apartment not found");
            }

            if (apartments.Any())
            {
                for (var index = 0; index < apartments.Count; index++)
                {
                    var apartment = apartments[index];

                    if (!apartment.IsApartmentFree(dateStart, dateEnd))
                    {
                        apartments.Remove(apartment);
                        index--;
                    }
                }
            }

            if (!apartments.Any())
            {
                throw new Exception("Free apartment not found");
            }

            if (apartments.Count == 1)
                return apartments[0].GetReference();

            return FindActualApartment(apartments, dateStart, dateEnd, service);
        }

        public bool IsApartmentFree(DateTime dateStart, DateTime dateEnd, Guid? currentBooking = null)
        {
            if (Id == null || Id == Guid.Empty)
            {
                throw new Exception("Id no specified");
            }

            if (currentBooking.HasValue && currentBooking == Guid.Empty)
            {
                throw new Exception($"{currentBooking} is empty Guid");
            }

            var query = new QueryExpression(BookingModel.LogicalName)
            {
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.Or,
                    Filters =
                    {
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id),
                                currentBooking.HasValue ? new ConditionExpression(BookingModel.Fields.PrimaryId, ConditionOperator.NotEqual, currentBooking) : null,
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.GreaterThan, dateStart),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.LessThan, dateEnd)
                            }
                        },
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id),
                                currentBooking.HasValue ? new ConditionExpression(BookingModel.Fields.PrimaryId, ConditionOperator.NotEqual, currentBooking) : null,
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.OnOrBefore, dateStart),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.OnOrAfter, dateEnd)
                            }
                        },
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.Equal, Id),
                                currentBooking.HasValue ? new ConditionExpression(BookingModel.Fields.PrimaryId, ConditionOperator.NotEqual, currentBooking) : null,
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.LessThan,
                                    dateStart),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.GreaterThan,
                                    dateEnd)
                            }
                        }
                    }
                }
            };

            var booking = _service.RetrieveMultiple(query).Entities.FirstOrDefault();

            return booking == null;
        }

        private static EntityReference FindActualApartment(List<Apartment> apartmentsList, DateTime dateStart, DateTime dateEnd, IOrganizationService service)
        {
            if (!apartmentsList.Any())
            {
                throw new Exception($"{nameof(apartmentsList)} is empty");
            }

            if (apartmentsList.Count == 1)
                return apartmentsList[0].GetReference();

            var id = apartmentsList.Where(a => a.Id.HasValue).Select(a => a.Id.Value).ToArray();

            var query = new QueryExpression(BookingModel.LogicalName)
            {
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.Or,
                    Filters =
                    {
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.In, id),
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.On, dateStart)
                            }
                        },
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.In, id),
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.On, dateEnd)
                            }
                        }
                    }
                },
                ColumnSet = new ColumnSet(BookingModel.Fields.Apartment)
            };

            var booking = service.RetrieveMultiple(query).ToEntityCollection<Booking>(service).FirstOrDefault();

            if (booking != null)
                return booking.Apartment;

            query = new QueryExpression(BookingModel.LogicalName)
            {
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.Or,
                    Filters =
                    {
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.In, id),
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.OnOrAfter, dateStart),
                                new ConditionExpression(BookingModel.Fields.DateEnd, ConditionOperator.OnOrBefore,  dateStart - (dateStart.AddDays(3) - dateStart))
                            }
                        },
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(BookingModel.Fields.Apartment, ConditionOperator.In, id),
                                new ConditionExpression(BookingModel.Fields.Type, ConditionOperator.NotEqual, BookingModel.TypeEnum.Отмененный),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.OnOrBefore, dateEnd),
                                new ConditionExpression(BookingModel.Fields.DateStart, ConditionOperator.OnOrAfter,  dateEnd.AddDays(3))
                            }
                        }
                    }
                },
                ColumnSet = new ColumnSet(BookingModel.Fields.Apartment)
            };

            booking = service.RetrieveMultiple(query).ToEntityCollection<Booking>(service).FirstOrDefault();

            return booking != null ? booking.Apartment : apartmentsList[0].GetReference();
        }
    }
}