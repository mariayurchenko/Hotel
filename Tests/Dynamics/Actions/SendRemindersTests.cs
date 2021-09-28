using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using SB.SharedModels.Actions;
using System.Collections.Generic;

namespace Tests.Dynamics.Actions
{
    [TestClass]
    public class SendRemindersTests : DynamicsTestBase<SB.Actions.ActionTracking>
    {
        [TestMethod]
        public void Successful()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.MessageName = ActionNames.ActionTracking;

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("ActionName", ActionNames.ActionTrackingNames.SendReminders),
                new KeyValuePair<string, object>("Parameters", string.Empty)
            };

            XrmRealContext.ExecutePluginWith<SB.Actions.ActionTracking>(executionContext);
        }
    }
}