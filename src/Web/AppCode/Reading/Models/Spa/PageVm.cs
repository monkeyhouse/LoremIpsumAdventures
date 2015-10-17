
using System;
using System.Collections.Generic;

namespace Models.Reading
{
    public class PageVm
    {
        public int PageId { get; set; }

        public int PageNumber { get; set; }

        public string Text { get; set; }

        public List<ActionVm> Actions { get; set; }
    }

    public class ActionVm
    {
        public string Text { get; set; }

        public int PageId { get; set; }
    }
}
