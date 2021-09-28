using System.Collections.Generic;

namespace SB.SharedModels.Apartments
{
    public class GetAvailableApartmentsResponse
    {
        public List<ApartmentTypeModel> ApartmentTypes { get; set; }
    }
}