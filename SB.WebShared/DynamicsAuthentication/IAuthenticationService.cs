using System;
using System.Threading.Tasks;

namespace SB.WebShared.DynamicsAuthentication
{
    public interface IAuthenticationService
    {
        public Task<string> GetToken();
        public Uri GetServiceUri();
        public string GetConnectionString();
    }
}