using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

//using System.Text.Json;

namespace SB.WebShared.DynamicsAuthentication
{
    public static class JsonCreatorService
    {
        public static StringContent FormStringContent<T>(T obj, string actionName)
        {
            var json = obj.GetType() != typeof(string) ? JsonConvert.SerializeObject(obj) : obj.ToString();
            var inputParameters = new InputParameters
            {
                ActionName = actionName,
                Parameters = json
            };
            var data = JsonConvert.SerializeObject(inputParameters);
            var result = new StringContent(data, Encoding.UTF8, "application/json");

            return result;
        }
        private class InputParameters
        {
            public string ActionName { get; set; }
            public string Parameters { get; set; }
        }
    }
}