using System.Collections.Generic;
using System.Runtime.Serialization;
using ININ.Alliances.DotNetIvr.Actions;

namespace ININ.Alliances.DotNetIvr
{
    [DataContract]
    [KnownType(typeof(DisconnectActionResponse))]
    [KnownType(typeof(GetDigitsActionResponse))]
    [KnownType(typeof(PlayActionResponse))]
    [KnownType(typeof(TransferToQueueActionResponse))]
    public class ActionResponse
    {
        #region Data members

        [DataMember]
        public string id { get; set; }

        #endregion



        #region Non-data member properties

        /// <summary>
        /// Sub options of this action (typically used for menu options)
        /// </summary>
        public List<ActionResponse> Children { get; set; }

        /// <summary>
        /// The menu digit associated with this item
        /// </summary>
        public string Digit { get; set; }

        #endregion


        public ActionResponse()
        {
            Children = new List<ActionResponse>();
            Digit = string.Empty;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(id) ? "[" + GetType().Name + "]" : id;
        }
    }
}