
using System;

namespace Models.Profile
{

    public class UserSummaryVm
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    
        public DateTime LastActive { get; set; }
        public DateTime JoinDate { get; set; }

    }

}
