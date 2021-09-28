using System;
using System.Collections.Generic;

namespace SB.SharedModels.Apartments
{
    public class Apartment
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MainImage { get; set; }

        public List<string> Images { get; set; }
    }
}