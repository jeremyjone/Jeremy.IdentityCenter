using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Database.Entities
{
    public enum Sex
    {
        Unknown = 0,
        Female,
        Male
    }

    public class IdcIdentityUser : IdentityUser<int>
    {
        /// <summary>
        /// 有效
        /// </summary>
        public bool Validity { get; set; } = true;

        /// <summary>
        /// 性别。0 为女，1 为男
        /// </summary>
        public Sex Sex { get; set; } = Sex.Unknown;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录 IP
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// 登录 UA
        /// </summary>
        public string LoginUserAgent { get; set; }

        public ICollection<IdcIdentityUserRole> UserRoles { get; set; }
    }
}
