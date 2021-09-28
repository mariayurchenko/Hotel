using FakeXrmEasy;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public abstract class DynamicsTestBase<T>
           where T : IPlugin, new()
    {
        protected readonly XrmRealContext XrmRealContext;
        private readonly int _recordsCount;

        protected DynamicsTestBase()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            Service = GetRealService(configuration);
            //Service = new CrmServiceClient(configuration.GetConnectionString("CRMConnectionString"));

            int.TryParse(configuration.GetSection("RecordsCount").Value, out _recordsCount);

            XrmRealContext = new XrmRealContext(Service);
        }

        public IOrganizationService Service { get; set; }

        internal List<Entity> GetRecords(QueryExpression query)
        {
            return GetRecords(query, _recordsCount);
        }

        protected List<Entity> GetRecords(QueryExpression query, int? count)
        {
            if (count == null)
            {
                return GetRecords(query, int.MaxValue);
            }

            return GetRecords(query, int.MaxValue);
        }

        protected List<Entity> GetRecords(QueryExpression query, int count)
        {
            var moreRecords = true;
            var records = new List<Entity>();

            if (query.PageInfo.Count == 0 || query.PageInfo.Count > count && count < 5000)
            {
                query.PageInfo.Count = count;
                query.PageInfo.PageNumber = 1;
            }

            while (moreRecords)
            {
                var results = Service.RetrieveMultiple(query);

                foreach (var entity in results.Entities)
                {
                    if (records.Count != count)
                    {
                        records.Add(entity);
                    }
                    else
                    {
                        return records;
                    }
                }

                moreRecords = results.MoreRecords;

                if (moreRecords)
                {
                    query.PageInfo.PagingCookie = results.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }

            return records;
        }

        private IOrganizationService GetRealService(IConfiguration configuration)
        {
            var clientId = configuration.GetValue<string>("ClientOptions:ClientId");
            var clientSecret = configuration.GetValue<string>("ClientOptions:ClientSecret");
            var resource = configuration.GetValue<string>("ClientOptions:Resource");
            var apiVersion = configuration.GetValue<string>("ClientOptions:ApiVersion");

            var authenticationParameters = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(resource.EndsWith("/") ? resource : $"{resource}/" + "api/data/" + apiVersion)).Result;
            var authContext = new AuthenticationContext(authenticationParameters.Authority, false);
            var clientCred = new ClientCredential(clientId, clientSecret);
            var auth = authContext.AcquireTokenAsync(authenticationParameters.Resource, clientCred)
                .GetAwaiter().GetResult();

            var serviceUrl = new Uri(authenticationParameters.Resource + @"XRMServices/2011/Organization.svc/web?SdkClientVersion=9.1");

            using (var sdkService = new OrganizationWebProxyClient(serviceUrl, false))
            {
                sdkService.HeaderToken = auth.AccessToken;
                // it does not seem to work w/o who am i request for some reason
                sdkService.Execute(new WhoAmIRequest());
                Service = sdkService;

                return sdkService;
            }
        }
    }
}