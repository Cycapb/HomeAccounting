namespace DomainModels.Model
{
    public class EmailSettings
    {
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public int ServerPort { get; set; }
        public bool UseSsl { get; set; }
    }
}