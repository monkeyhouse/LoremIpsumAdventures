using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.WebEncoders;
using System;
using PagedList;
using Models.Reading;
using Models.Meta;
using FakeData;

namespace Web.Controllers
{

    [Route("Read/[Action]")]
    public class ReadController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string genre = "", string filter = "Hot", int? page = 1)
        {
            var pageSize = 10;
            int totalCount = 40;
            var stories = Provider<StoryVm>.Generate(pageSize).ToList();

            var storiesAsIPagedList = new StaticPagedList<StoryVm>(stories, page.Value, pageSize, totalCount);

            ViewBag.OnePageOfStories = storiesAsIPagedList;

            var genres = new MenuDropdownService().GetGenres(genre);
            ViewBag.Genres = genres;
            ViewBag.SelectedGenre = genres.First(t => t.Selected == true);
            ViewBag.SelectedFilter = filter;


            return View();
            /*      
               todo: view model?
                public class BrowseVm
                {
                    public IEnumerable<SelectListItem> Genres { get; set;}

                    public IEnumerable<StoryVm> Stories { get; set; }
                } 
            */
        }

        [HttpGet("{storyId}")]
        public IActionResult Details(int storyId)
        {
            var story = Provider<StoryVm>.Generate(1).First();

            var stats = DetailProvider<PlayerStoryStatsVm>.Generate();

            var vm = new DetailsVm()
            {
                Stats = stats,
                Story = story
            };

            return View(vm);
        }


        public IActionResult Preview(int storyId)
        {
            var preview = DetailProvider<PreviewVm>.Generate();

            return PartialView(preview);
        }

        public IActionResult Play(int storyId)
        {                      
            return View();
        }

        public IActionResult PlayStyleDemo()
        {
            return View();
        }

        
    }


    public class MenuDropdownService
    {
        public IEnumerable<SelectListItem> GetGenres(string selectedValue) {

            var genres = Provider<GenreVm>.Generate().Select( t => new SelectListItem()
            { Text = t.Name,
              Value = t.Name //UrlEncode(t.Name)
            });

            var allOpts = new List<SelectListItem>();

            if (FakeData.Profile.IsLoggedIn)
                allOpts.Add(new SelectListItem { Text = "My Favorites Genres", Value = "!Favorites!" + FakeData.Profile.MyFavoriteGenres });

            allOpts.Add(new SelectListItem { Text = "All", Value = "All" });

            allOpts.AddRange(genres);


            //set seleted value
            if (String.IsNullOrEmpty(selectedValue) || selectedValue.IndexOf(",") > 0)
            {
                //select favorite genres
                allOpts.First().Selected = true;
            }else {
                allOpts.First(t => t.Value == selectedValue).Selected = true;
            }

            return allOpts;

        }
    }



    [Route("api/Read/[action]")]
    public class ReadApiController : Controller
    {
        [HttpGet("{storyId}")]
        public JsonResult Cover( int storyId )
        {
            var cover = DetailProvider<CoverVm>.Generate();

            foreach( var a in cover.Authors)
            {
                var str = Url.Action("Summary", "Profile", new { userId = a.ProfileId });
                a.ProfileUrl = str;
            }
           
            return Json(cover);
        }

        [HttpGet("{storyId}/{pageId}")]
        public JsonResult Page( int storyId, int pageId)
        {
            var page = DetailProvider<PageVm>.Generate();

            return Json(page);
        }

        [HttpGet("{storyId}")]
        public JsonResult Jacket(int storyId)
        {
            //return multiple sections?
            var section = DetailProvider<JacketVm>.Generate();

            return Json(section);
        }

    }
}
