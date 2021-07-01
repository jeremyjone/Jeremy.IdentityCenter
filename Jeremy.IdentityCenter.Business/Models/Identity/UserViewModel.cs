using Jeremy.IdentityCenter.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserViewModel : IBaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Display(Name = "已确认邮箱")]
        public bool EmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [Display(Name = "手机号码已确认")]
        public bool PhoneNumberConfirmed { get; set; }

        [MaxLength(30)]
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Display(Name = "性别")] public Sex Sex { get; set; } = Sex.Unknown;

        [Display(Name = "头像")]
        public string Avatar { get; set; }

        [Display(Name = "出生日期")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "上次登录时间")]
        public DateTime LoginTime { get; set; }

        [Display(Name = "上次登录IP")]
        public string LoginIp { get; set; }

        [Display(Name = "上次登录UA")]
        public string LoginUserAgent { get; set; }

        [Display(Name = "启用锁定")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "启用二次验证")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "访问失败次数")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "锁定结束时间")]
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
