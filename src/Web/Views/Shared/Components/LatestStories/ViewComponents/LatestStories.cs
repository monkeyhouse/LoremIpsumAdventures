using Microsoft.AspNet.Mvc;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Web.ViewComponents
{
 
    public class LatestStories : ViewComponent
    {

        IEnumerable<StoryVm> stories; 

        public LatestStories()
        {
            stories = Provider<StoryVm>.Generate(5);
        }

        public IViewComponentResult Invoke()
        {
            foreach (var story in stories)
            {
                story.PublishDate = System.DateTime.Today.AddDays( -1 * Faker.RandomNumber.Next(1,10) );
            }

            return View( stories.OrderByDescending( s => s.PublishDate ) );
        }

    }
}
