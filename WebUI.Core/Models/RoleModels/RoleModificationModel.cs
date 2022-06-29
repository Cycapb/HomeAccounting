﻿namespace WebUI.Core.Models.RoleModels
{
    public class RoleModificationModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] UsersToAdd { get; set; }
        public string[] UsersToDelete { get; set; }
    }
}
