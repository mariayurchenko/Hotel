using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.SharedModels.Actions;
using SB.SharedModels.Apartments;
using System;
using System.Collections.Generic;
using JsonSerializer = SB.Shared.JsonSerializer;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class GetAvailableApartmentsTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var request = new GetAvailableApartmentsRequest
            {
                DateStart = DateTime.Now.ToShortDateString(),
                DateEnd = DateTime.Now.AddDays(5).ToShortDateString()
            };

            var json = JsonSerializer.Serialize(request);

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.GetAvailableApartments),
                new KeyValuePair<string, object>("Parameters", json)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}