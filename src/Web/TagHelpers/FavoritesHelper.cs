using Humanizer;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;


namespace TagHelpers
{

    [TargetElement("span", Attributes = FavoritesAttributes)]
    public class FavoritesHelper : TagHelper
    {
        private const string FavoritesAttributes = "num-favorites";

        [HtmlAttributeName("num-favorites")]
        public int favorites { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var str = $@"<span> {favorites} <span class='glyphicon glyphicon-heart-empty' title='{favorites} favorites'></span> </span> ";
            output.Content.Append(str);
        }
    }
}
