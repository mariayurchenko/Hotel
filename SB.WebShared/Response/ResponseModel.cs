using System.Runtime.Serialization;

namespace SB.WebShared.Response
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember(Name = "@odata.context")]
        public string OdataContext { get; set; }
        [DataMember(Name = "Response")]
        public string Response { get; set; }
    }
}