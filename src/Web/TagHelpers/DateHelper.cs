using Humanizer;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;


namespace TagHelpers
{

    [TargetElement("span", Attributes = DateAttributeName)]
    public class DateHelper : TagHelper
    {
        private const string DateAttributeName = "human-date";
        private const string RemoveSuffixName = "remove-suffix";

        [HtmlAttributeName(DateAttributeName)]
        public DateTime DateValue { get; set; }

        [HtmlAttributeName(RemoveSuffixName)]
        public bool RemoveSuffix { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {            
            var dateText = DateValue.Humanize();

            if (RemoveSuffix)
            {
                output.Content.Append(dateText.Replace(" ago", "").Replace(" from now", ""));
                return;
            }

            output.Content.Append(dateText);
        }
    }
}
