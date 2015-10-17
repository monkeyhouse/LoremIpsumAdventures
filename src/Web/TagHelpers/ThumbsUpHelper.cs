using Humanizer;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;


namespace TagHelpers
{

    [TargetElement("span", Attributes = ThumbsAttributes)]
    public class ThumbsUpHelper : TagHelper
    {
        private const string ThumbsAttributes = "up-votes,down-votes";
        private const string Likes = "up-votes";
        private const string Dislikes = "down-votes";

        [HtmlAttributeName(Likes)]
        public int likes { get; set; }

        [HtmlAttributeName(Dislikes)]
        public int dislikes { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var difference = likes - dislikes;
            var icon = difference > 0 ? "glyphicon-thumbs-up" :
                       difference < 0 ? "glyphicon-thumbs-down" : "";

            var str = $@"<span title = '{likes} like, {dislikes} dislike'> {difference} 
                         <span class='glyphicon {icon}'></span> </span> ";
            output.Content.Append( str );
        }
    }

}
