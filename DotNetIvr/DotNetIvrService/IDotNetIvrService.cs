using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using ININ.Alliances.DotNetIvr.Actions;

namespace ININ.Alliances.DotNetIvr
{
    [ServiceContract]
    public interface IDotNetIvrService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST", 
            UriTemplate = "/nextaction",
            BodyStyle = WebMessageBodyStyle.Bare, 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json)]
        [Description("Calls a specific number and plays a text to speech message to them")]
        ActionResponse NextAction(NextActionRequest request);
    }
}
