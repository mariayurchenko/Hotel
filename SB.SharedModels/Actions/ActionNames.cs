namespace SB.SharedModels.Actions
{
    public class ActionNames
    {
        public const string ActionTracking = "sb_ActionTracking";
        public class ActionTrackingNames
        {
            public const string GetCurrentPrice = nameof(GetCurrentPrice);
            public const string UpdateBookingHistory = nameof(UpdateBookingHistory);
            public const string CalculatePrice = nameof(CalculatePrice);
            public const string CloseOldBookings = nameof(CloseOldBookings);
            public const string SendReminders = nameof(SendReminders);
            public const string CreateBooking = nameof(CreateBooking);
            public const string GetAvailableApartments = nameof(GetAvailableApartments);
            public const string GetApartment = nameof(GetApartment);
            public const string GetAllApartments = nameof(GetAllApartments);
            public const string GetPriceIfApartmentTypeAvailable = nameof(GetPriceIfApartmentTypeAvailable);
        }
    }
}