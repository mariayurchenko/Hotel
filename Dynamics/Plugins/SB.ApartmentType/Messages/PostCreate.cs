using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using System;

namespace SB.ApartmentType.Messages
{
    public class PostCreate : IPlugin
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

                var price = new Price(service)
                {
                    DateStart = DateTime.Now,
                    PriceValue = apartmentType.CurrentPrice,
                    Name = Price.CreateName(apartmentType.Name),
                    ApartmentType = apartmentType.GetReference()
                };

                price.Save();
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}