using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.SharedModels.Actions;
using SB.SharedModels.Booking;
using System;
using System.Collections.Generic;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class CreateBookingTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var booking = new CreateBookingRequest
            {
                ApartmentId = new Guid("290e73c6-750a-ec11-b6e6-00224882aa78"),
                Adults = 2,
                DateStart = DateTime.Now.ToShortDateString(),
                DateEnd = DateTime.Now.AddDays(5).ToShortDateString(),
                Price = 1000,
                ClientName = "Bob Smit",
                PhoneNumber = "38085678998",
                Email = "maria.yurchenko17@gmail.com"
            };

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.CreateBooking),
                new KeyValuePair<string, object>("Parameters", JsonSerializer.Serialize(booking))
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}