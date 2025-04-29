using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PetIsland.Utility;

[HtmlTargetElement("label", Attributes = "asp-for")]
public class RequiredLabelTagHelper : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public required ModelExpression For { get; set; }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);

        if (For.Metadata.IsRequired)
        {
            var requiredIndicator = new TagBuilder("span");
            requiredIndicator.AddCssClass("text-danger");
            requiredIndicator.InnerHtml.Append("*");
            output.Content.AppendHtml(requiredIndicator);
        }
    }
}