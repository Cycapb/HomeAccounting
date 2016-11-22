using HomeAccountingSystem_DAL.Abstract;

namespace HomeAccountingSystem_WebUI.Models
{
    public class WebUser : IWorkingUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }
}