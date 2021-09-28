using Microsoft.Xrm.Sdk;
using System;

namespace SB.ApartmentType.Messages
{
    public class PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var target = (Entity)context.InputParameters["Target"];

                var apartmentType = new Shared.EntityProviders.ApartmentType(target, service);

                if (string.IsNullOrWhiteSpace(apartmentType.Name))
                {
                    throw new Exception($"{nameof(apartmentType.Name)} is null or empty white-space");
                }
                if (apartmentType.CurrentPrice == null)
                {
                    throw new Exception($"{nameof(apartmentType.CurrentPrice)} is null");
                }
                if (apartmentType.CurrentPrice <= 0)
                {
                    throw new Exception($"{nameof(apartmentType.CurrentPrice)} should be more than 0");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}