using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebUI.Core.Infrastructure.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "[ajax-enable=true]")]
    [HtmlTargetElement("form", Attributes = "[ajax-enable=true]")]
    public class DataAjaxTagHelper : TagHelper
    {
        public bool AjaxEnable { get; set; }

        public string AjaxMode { get; set; }

        public string AjaxUpdateElementId { get; set; }

        public string AjaxUrl { get; set; }

        public string AjaxSuccess { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-ajax", "true");
            
            if (string.IsNullOrWhiteSpace(AjaxMode))
            {
                output.Attributes.Add("data-ajax-mode", "replace");
            }
            else
            {
                output.Attributes.Add("data-ajax-mode", AjaxMode);
            }

            output.Attributes.Add("data-ajax-update", AjaxUpdateElementId);
            output.Attributes.Add("data-ajax-url", AjaxUrl);

            if (!string.IsNullOrWhiteSpace(AjaxSuccess))
            {
                output.Attributes.Add("data-ajax-success", AjaxSuccess);
            }
        }
    }
}
