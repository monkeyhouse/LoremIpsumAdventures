

using Models.Search;
using System.Collections.Generic;

namespace Models.Profile
{
    public class ProfileSummaryOuterVm
    {
        public UserSummaryVm User { get; set; }
        public ProfileStoryScope StoryScope { get; set; }
        public ProfileStorySort StorySort { get; set; }
        public IEnumerable<UserSearchVm> Following { get; set; }
        public IEnumerable<UserSearchVm> Followers { get; set; }
        public StoriesStatsVm Stats { get; set; }
    }

    public class ProfileSummaryVm
    {
        public IEnumerable<StoryVm> Stories { get; set; }
        public StoriesStatsVm Stats { get; set; }
        public UserSummaryVm User { get; set; }
        public ProfileStoryScope StoryScope { get; set; }
        public ProfileStorySort StorySort { get; set; }

    }


    public enum ProfileStoryScope
    {
        All = 0,
        Author = 1,
        Coauthor = 2
    }

    public enum ProfileStorySort
    {
        Top = 0,
        Newest = 1,
        InProgress = 2
    }
}
