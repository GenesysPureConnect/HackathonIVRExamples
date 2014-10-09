using System.Runtime.Serialization;

namespace ININ.Alliances.DotNetIvr.Actions
{
    [DataContract]
    public class GetDigitsActionResponse : ActionResponse
    {
        [DataMember]
        public string action { get { return "getdigits"; } set { } }
    }
}
