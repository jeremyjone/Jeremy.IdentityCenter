using System;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.PersistedGrants
{
    public class PersistedGrantViewModel
    {
        [Display(Name = "键")]
        public string Key { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "主体标识")]
        public string SubjectId { get; set; }

        [Display(Name = "主体名称")]
        public string SubjectName { get; set; }

        [Display(Name = "Session 标识")]
        public string SessionId { get; set; }

        [Display(Name = "客户端 ID")]
        public string ClientId { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "到期时间")]
        public DateTime? Expiration { get; set; }

        [Display(Name = "消耗时间")]
        public DateTime? ConsumedTime { get; set; }

        [Display(Name = "数据")]
        public string Data { get; set; }
    }
}
