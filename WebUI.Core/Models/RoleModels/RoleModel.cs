using System.Collections.Generic;
using WebUI.Core.Models.UserModels;

namespace WebUI.Core.Models.RoleModels
{
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
