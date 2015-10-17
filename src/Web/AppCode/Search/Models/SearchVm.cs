using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Models.Search
{
    public class SearchModel
    {
        public string Q { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> Genres { get; set; }


        public List<SelectListItem> SortOptions { get; set; }
        public SelectListItem SelectedSortOption { get; set; }


        public const string StoryType = "Stories";
        public const string UsersType = "Users";
    }



}
