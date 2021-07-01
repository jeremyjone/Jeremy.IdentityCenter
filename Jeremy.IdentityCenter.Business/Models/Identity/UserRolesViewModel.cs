using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserRolesViewModel : IBaseViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "角色 ID")]
        public int RoleId { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        public List<SelectItemViewModel> RolesList { get; set; } = new List<SelectItemViewModel>();

        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
