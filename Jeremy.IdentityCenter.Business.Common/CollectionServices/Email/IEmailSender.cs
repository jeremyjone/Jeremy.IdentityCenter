using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public interface IEmailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="address">收件人地址</param>
        /// <param name="displayName">显示的名称</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isBodyHtml">邮件是否包含 HTML。默认为 false</param>
        /// <returns></returns>
        public Task SendEmailAsync(string address, string displayName, string subject, string body, bool isBodyHtml = false);
    }
}