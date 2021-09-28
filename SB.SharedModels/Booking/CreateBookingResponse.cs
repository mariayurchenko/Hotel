using System;

namespace SB.SharedModels.Booking
{
    public class CreateBookingResponse
    {
        public Guid BookingId { get; set; }

        public string BookingNumber { get; set; }
    }
}