using System;
using System.Collections.Generic;

namespace Models.Reading
{
    public class PreviewVm
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public int StoryId { get; set; }

        public List<PreviewPageVm> Pages { get; set; }
    }

    public class PreviewPageVm
    {
        public int PageNumber { get; set; }

        public string Body { get; set; }

        public List<String> Actions { get; set; }
    }

}
