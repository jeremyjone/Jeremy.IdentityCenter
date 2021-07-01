using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Helpers
{
    [HtmlTargetElement("label-name")]
    public class LabelNameTagHelper : LabelTagHelper
    {
        public LabelNameTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            var desc = For.Metadata.Name;
            if (!string.IsNullOrWhiteSpace(For.Metadata.Description)) desc = For.Metadata.Description;
            else if (!string.IsNullOrWhiteSpace(For.Metadata.DisplayName)) desc = For.Metadata.DisplayName;

            //// 添加描述图标
            if (!string.IsNullOrWhiteSpace(desc))
            {
                var icon = new TagBuilder("i");
                icon.Attributes.Add("class", "fa fa-comment");
                icon.Attributes.Add("style", "font-size: 12px;");

                var span = new TagBuilder("span");
                span.Attributes.Add("class", "ml-1");
                span.Attributes.Add("data-toggle", "tooltip");
                span.Attributes.Add("data-original-title", desc);
                span.InnerHtml.AppendHtml(icon);

                output.Content.AppendHtml(span);
            }
        }
    }
}
