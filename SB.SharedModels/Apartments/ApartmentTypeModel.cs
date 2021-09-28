using System;

namespace SB.SharedModels.Apartments
{
    public class ApartmentTypeModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int? Price { get; set; }

        public string MainImage { get; set; }
    }
}