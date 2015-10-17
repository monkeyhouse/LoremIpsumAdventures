using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Models;
using System.Collections.Generic;
using System.Linq;
using System;
using PagedList;
using Models.Search;
using Web.Util;

namespace Web.Controllers
{

    public class SearchController : Controller
    {
     

        public IActionResult Index(string q = "",
                                   string type = SearchModel.StoryType,
                                   string genres = "",
                                   int sort = 0,
                                   int? p = 1)
        {
            var pageSize = 10;
            int totalCount = 40;


            if (type == SearchModel.StoryType)
            {
                var stories = Provider<StoryVm>.Generate(pageSize).ToList();
                var storiesAsIPagedList = new StaticPagedList<StoryVm>(stories, p.Value, pageSize, totalCount);
                ViewBag.OnePage = storiesAsIPagedList;
                ViewBag.ResultCount = totalCount;
            }
            else
            {
                var users = Provider<UserSearchVm>.Generate(pageSize).ToList();
                var usersAsPagedList = new StaticPagedList<UserSearchVm>(users, p.Value, pageSize, totalCount);
                ViewBag.OnePage = usersAsPagedList;
                ViewBag.ResultCount = totalCount;
            }


            HashSet<int> genreHash;
            if (!string.IsNullOrEmpty(genres))            {
                genreHash = new HashSet<int>(genres.Split('~').Select(t => Convert.ToInt32(t)));
            }else{
                genreHash = new HashSet<int>();
            }

            var allGenres = Provider<GenreVm>.Generate().Select(
                                t => new SelectListItem() {
                                    Value = t.Id.ToString(),
                                    Text = t.Name,
                                    Selected = genreHash.Contains(t.Id)
                                }).ToList();

            var sortOptions = EnumExtensions.GetEnumSelectList<SearchSortType>().ToList();       
            var selectedSortOption = sortOptions.FirstOrDefault(t => t.Value == sort.ToString());
           

            var model = new SearchModel();
            model.Q = q;
            model.Type = type;
            model.Genres = allGenres;
            model.SortOptions = sortOptions;
            model.SelectedSortOption = selectedSortOption;

            return View(model);
        }


    }

}
