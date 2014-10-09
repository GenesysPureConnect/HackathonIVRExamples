namespace ININ.Alliances.DotNetIvr
{
    public class NextActionRequest
    {
        public string callid { get; set; }
        public string remotename { get; set; }
        public string remotenumber { get; set; }
        public string lastdigitsreceived { get; set; }
        public string lastactionid { get; set; }
    }
}