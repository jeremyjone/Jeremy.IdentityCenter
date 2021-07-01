using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Jeremy.IdentityCenter.Business.Helpers
{
    [HtmlTargetElement("avatar")]
    public class AvatarTagHelper : TagHelper
    {
        public IIdentityService IdentityService { get; }

        public AvatarTagHelper(IIdentityService identityService)
        {
            IdentityService = identityService;
        }

        [HtmlAttributeName("user-id")]
        public string Id { get; set; }

        [HtmlAttributeName("src")]
        public string Src { get; set; }

        [HtmlAttributeName("alt")]
        public string Alt { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [HtmlAttributeName("size")] public int Size { get; set; } = 150;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (!string.IsNullOrWhiteSpace(Class))
            {
                output.Attributes.Add("class", Class);
            }

            if (!string.IsNullOrWhiteSpace(Alt))
            {
                output.Attributes.Add("alt", Alt);
            }

            if (Size != default)
            {
                output.Attributes.Add("width", Size);
            }

            var avatar = "/img/avatar.jpg";
            if (!string.IsNullOrWhiteSpace(Src))
            {
                avatar = Src;
            }
            else if (int.TryParse(Id, out var userId) && userId != default)
            {
                var value = IdentityService.GetUserAsync(userId).Result.Avatar;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    avatar = value;
                }
            }

            output.Attributes.Add("src", GetAvatarUrl(avatar, Size));
            output.TagMode = TagMode.SelfClosing;
        }

        private static string GetAvatarUrl(string hash, int size)
        {
            var sizeArg = size > 0 ? $"?s={size}" : "";

            return $"{hash}{sizeArg}";
        }
    }
}
