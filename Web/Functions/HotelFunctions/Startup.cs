using HotelFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SB.WebShared.DynamicsAuthentication;
using SB.WebShared.Interactors;
using System;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Startup))]

namespace HotelFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

            if (config == null)
            {
                throw new Exception($"{nameof(config)} is null");
            }

            if (new CrmClientOptions().BindStringMembersFromConnectionString(config["ClientOptions"]) is not CrmClientOptions crmOptions)
            {
                throw new Exception($"{nameof(crmOptions)} is null");
            }

            builder.Services
                .AddSingleton(crmOptions)
                .AddSingleton(new HttpClient())
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IDynamicsInteractor, DynamicsInteractor>();
        }
    }
}