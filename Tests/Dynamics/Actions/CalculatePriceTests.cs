using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.SharedModels.Actions;
using System.Collections.Generic;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class CalculatePriceTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            var body = "012fe967-b60c-ec11-b6e6-00224882aa78";

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.CalculatePrice),
                new KeyValuePair<string, object>("Parameters", body)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}