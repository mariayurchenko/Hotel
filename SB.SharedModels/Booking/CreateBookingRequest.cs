using System;

namespace SB.SharedModels.Booking
{
    public class CreateBookingRequest
    {
        public Guid ApartmentId { get; set; }

        public string DateStart { get; set; }

        public string DateEnd { get; set; }

        public int Adults { get; set; }

        public int? Children { get; set; }

        public int? Price { get; set; }

        public string ClientName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }
}