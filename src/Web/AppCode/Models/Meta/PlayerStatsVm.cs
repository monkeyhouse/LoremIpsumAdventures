using System;

namespace Models.Meta
{

    //stats per a story, per user
    // psStatsVm
    public class PlayerStoryStatsVm
    {
        public int Plays { get; set; }

        public int EndingCompleted { get; set; }


        public int MinutesPlayed { get; set; }

        public int EndingsAvailable { get; set; }



        // these stats may be too invasive or detailed   
        //public int PagesSeen { get; set; }
        // public int ActionsTaken { get; set; }
        //public int PagesAvailable { get; set; }
        //public int ActionsAvailable { get; set; }
        //<tr><td>Total Duration    </td><td> 24 min</td></tr>
        //Pages Seen        </td><td> 20      </td></tr>
        //Actions Taken     </td><td> 31 / 40 </td></tr>*@

        //public int Id { get; set; }
        //public int StoryId { get; set; }
        //public int UserId { get;set; }
    }

}
