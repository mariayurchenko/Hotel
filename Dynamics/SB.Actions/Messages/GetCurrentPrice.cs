using Microsoft.Xrm.Sdk;
using SB.Shared.EntityProviders;
using System;

namespace SB.Actions.Messages
{
    public class GetCurrentPrice : IActionTracking
    {
        private readonly IOrganizationService _service;

        public GetCurrentPrice(IOrganizationService service)
        {
            _service = service;
        }

        public void Execute(string parameters, out string response)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                throw new Exception($"{nameof(parameters)} is null or white-space");
            }

            if (!Guid.TryParse(parameters, out var id))
            {
                throw new Exception($"{nameof(parameters)} is not parse to Guid");
            }

            var price = Price.GetPriceByDate(id, DateTime.Now, _service);

            response = price.ToString();
        }
    }
}