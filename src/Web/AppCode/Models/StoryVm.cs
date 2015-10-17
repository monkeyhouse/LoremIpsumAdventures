
using System;

namespace Models
{
    public class StoryVm
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string Genre { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime LastUpdated { get; set; }

        //Computed Data
        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public int Favorites { get; internal set; }
        public string[] Genres { get; internal set; }
    }

}
