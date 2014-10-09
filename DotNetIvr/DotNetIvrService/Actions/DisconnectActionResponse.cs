using System.Runtime.Serialization;

namespace ININ.Alliances.DotNetIvr.Actions
{
    [DataContract]
    public class DisconnectActionResponse : ActionResponse
    {
        [DataMember]
        public string action { get { return "disconnect"; } set { } }
    }
}