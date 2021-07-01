using Jeremy.IdentityCenter.Business.Common.CollectionServices.Email;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Handlers
{
    public class IdcEmailSender : EmailSender
    {
        private string DisplayName { get; }
        public IdcEmailSender(IConfiguration configuration, EmailOptions options) : base(configuration, options)
        {
            DisplayName = configuration["Email:DisplayName"];
        }

        /// <summary>
        /// 发送确认邮件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="link"></param>
        public async Task SendConfirmationEmailAsync(string address, string link)
        {
            const string subject = "确认邮箱";
            var body = "请点击以下链接：<br />" +
                       $"<a href='{link}' target=\"_blank\">{link}</a>" +
                       "<br /><br /> 如果这不是您操作的，请不要点击。如果对此链接有疑问，请联系我们：" +
                       Configuration["Url:ContactMe"];

            await SendEmailAsync(address, DisplayName, subject, body, true);
        }

        /// <summary>
        /// 发送重置密码邮件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task SendResetPasswordEmailAsync(string address, string link)
        {
            const string subject = "确认重置连接";
            var body = "点击以下链接以重置您的密码：<br />" +
                       $"<a href='{link}' target=\"_blank\">{link}</a>" +
                       "<br /><br /> 如果这不是您操作的，请不要点击。如果对此链接有疑问，请联系我们：" +
                       Configuration["Url:ContactMe"];

            await SendEmailAsync(address, DisplayName, subject, body, true);
        }
    }
}
