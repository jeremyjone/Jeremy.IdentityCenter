using Jeremy.IdentityCenter.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码至少{2}位")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次密码不匹配")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "性别")]
        [EnumDataType(typeof(Sex))]
        public string Sex { get; set; }

        public Sex SexEnum => Enum.TryParse(Sex, true, out Sex result) ? result : Database.Entities.Sex.Unknown;

        public List<SelectItemViewModel> SexList { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = DateTime.Now;
    }
}
