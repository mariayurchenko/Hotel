using Microsoft.Xrm.Sdk;
using System;

namespace SB.Price.Messages
{
    public class PreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var price = new Shared.EntityProviders.Price(target, service);

                if (price.PriceValue == null || price.PriceValue <= 0)
                {
                    throw new Exception($"{nameof(price.PriceValue)} should be more than 0");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}