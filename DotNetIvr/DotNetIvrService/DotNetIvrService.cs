using System;
using System.ServiceModel;
using ININ.Alliances.DotNetIvr.Actions;

namespace ININ.Alliances.DotNetIvr
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DotNetIvrService : IDotNetIvrService
    {
        public ActionResponse NextAction(NextActionRequest request)
        {
            try
            {
                // Get next action
                var nextAction = MenuManager.GetNextAction(
                    string.IsNullOrEmpty(request.lastactionid) ? "" : request.lastactionid,
                    string.IsNullOrEmpty(request.lastdigitsreceived) ? "" : request.lastdigitsreceived);
                
                // Trace info about request and response
                Console.WriteLine("{4} NextAction: {0}|{1}|{2} -> Result: {3}", request.callid, request.lastactionid, request.lastdigitsreceived, nextAction, DateTime.Now.ToString("HH:mm:ss"));

                return nextAction;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in NextAction: " + ex.Message);
                return new DisconnectActionResponse();
            }
        }
    }
}