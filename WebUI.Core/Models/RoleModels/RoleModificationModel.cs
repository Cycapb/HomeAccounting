namespace WebUI.Core.Models.RoleModels
{
    public class RoleModificationModel
    {
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
