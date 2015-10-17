using Microsoft.AspNet.Mvc;
using Models;
using Models.Profile;
using System.Collections.Generic;
using System.Linq;

namespace Profile.ViewComponents
{
 
    public class StoriesVC : ViewComponent
    {      
        public StoriesVC()
        {
        }

        public IViewComponentResult Invoke(int UserId, ProfileStoryScope scope, ProfileStorySort sort)
        {
            var vm = new ProfileSummaryVm();
            vm.Stats = DetailProvider<StoriesStatsVm>.Generate();
            vm.Stories = Provider<StoryVm>.Generate(5);
            vm.User = DetailProvider<UserSummaryVm>.Generate();
            vm.StoryScope = scope;
            vm.StorySort = sort;

            return View( "../Default", vm );
        }

    }


}

