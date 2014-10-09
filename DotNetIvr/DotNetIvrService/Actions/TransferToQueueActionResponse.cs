using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ININ.Alliances.DotNetIvr.Actions
{
    [DataContract]
    public class TransferToQueueActionResponse : ActionResponse
    {
        [DataMember]
        public string action { get { return "transfertoqueue"; } set { } }
    }
}
