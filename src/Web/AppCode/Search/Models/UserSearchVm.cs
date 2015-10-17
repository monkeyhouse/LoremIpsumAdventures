using System;

namespace Models.Search
{

    public class UserSearchVm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StoriesWrittenTo { get; set; }
        //written to n stories

        public int Followers { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime LastActive { get; set; }
    }

}
