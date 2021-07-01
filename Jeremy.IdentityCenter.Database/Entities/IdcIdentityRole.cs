using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Database.Entities
{
    public class IdcIdentityRole : IdentityRole<int>
    {
        /// <summary>
        /// 有效
        /// </summary>
        public bool Validity { get; set; } = true;

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public ICollection<IdcIdentityUserRole> UserRoles { get; set; }
    }
}