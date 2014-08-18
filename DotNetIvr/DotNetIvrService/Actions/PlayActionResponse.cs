using System.Runtime.Serialization;

namespace ININ.Alliances.DotNetIvr.Actions
{
    [DataContract]
    public class PlayActionResponse : ActionResponse
    {
        [DataMember] public string action {get { return "play"; } set{}}

        [DataMember]
        public string message { get; set; }
    }
}