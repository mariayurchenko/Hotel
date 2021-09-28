using System;

namespace SB.SharedModels.Apartments
{
    public class GetPriceIfApartmentTypeAvailableRequest
    {
        public Guid ApartmentTypeId { get; set; }

        public string DateStart { get; set; }

        public string DateEnd { get; set; }
    }
}