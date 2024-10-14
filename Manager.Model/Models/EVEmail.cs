namespace Manager.Model.Models
{
    public class EVEmail
    {
        public string Program { get; set; }
        public string hostName { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool useSSL { get; set; }
        public string templateUrl { get; set; }
        public string MAIL { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
    }
}
