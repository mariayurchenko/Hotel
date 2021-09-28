using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.Shared;
using SB.SharedModels.Actions;
using SB.SharedModels.Apartments;
using System;
using System.Collections.Generic;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class GetPriceIfApartmentTypeAvailableTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var parameters = new GetPriceIfApartmentTypeAvailableRequest
            {
                ApartmentTypeId = new Guid("290e73c6-750a-ec11-b6e6-00224882aa78"),
                DateStart = DateTime.Now.AddDays(100).ToShortDateString(),
                DateEnd = DateTime.Now.AddDays(105).ToShortDateString()
            };

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.GetPriceIfApartmentTypeAvailable),
                new KeyValuePair<string, object>("Parameters", JsonSerializer.Serialize(parameters))
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}