using System.Linq;
using Microsoft.AspNet.Mvc;
using FakeData;
using Models;
using ActionResults;
using Models.Profile;
using Models.Search;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        public IActionResult Aurelia()
        {
            return View();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Aurelia");
        }
        
        /// <summary>
        /// Provides summary to other users
        /// </summary>
        [HttpGet("{userId}")]
        public IActionResult Summary(int userId)
        {
            var vm = new ProfileSummaryOuterVm();
            vm.User = DetailProvider<UserSummaryVm>.Generate();
            vm.StoryScope = ProfileStoryScope.All;
            vm.StorySort = ProfileStorySort.Top;
            vm.Followers = Provider<UserSearchVm>.Generate(5);
            vm.Following = Provider<UserSearchVm>.Generate(5);
            vm.Stats = DetailProvider<StoriesStatsVm>.Generate();

            return View( vm );
        }

        //Partial Results
        public IActionResult GetStories(int userId, ProfileStoryScope scope, ProfileStorySort sort)
        {
            var vm = new ProfileSummaryVm();
            vm.Stories = Provider<StoryVm>.Generate(5);
            vm.User = DetailProvider<UserSummaryVm>.Generate();
            vm.StoryScope = scope;
            vm.StorySort = sort;

            return PartialView("Components/Default", vm );
        }

        public IActionResult GetStats(int userId, ProfileStoryScope scope, ProfileStorySort sort)
        {
            var stats =  DetailProvider<StoriesStatsVm>.Generate();
            return PartialView("ProfileStats", stats);
        }


    }
    


    [Route("api/[controller]/[action]")]
    public class FavoriteGenres : Controller
    {
        public IActionResult GetAllGenres()
        {


            var favGenres = FakeData.Profile.MyFavoriteGenreVms.Select(t => t.Id).ToArray();

            var allGenres = Provider<GenreVm>.Generate()
                .Select(t => new FavGenreVm()
                {
                    GenreId = t.Id,
                    Name = t.Name,
                    Description = t.Description
                }).ToList();

            foreach (var g in allGenres)
            {
                g.IsFavorite = favGenres.Contains(g.GenreId);
            }

            var j = allGenres.Select(t => new { id = t.GenreId, name = t.Name, desc = t.Description, isFav = t.IsFavorite});
            return Json(j);
        }
        
        [HandleUIException]
        [HttpPost]
        public IActionResult Add(int genreId) {
            FakeData.Profile.AddGenre(genreId);
            return new HttpStatusCodeResult(200);
        }

        [HandleUIException]
        [HttpPost]
        public IActionResult Remove(int genreId)
        {
            FakeData.Profile.RemoveGenre(genreId);
            return new HttpStatusCodeResult(200);
        }



     
    }



}
