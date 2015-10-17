
using System;
using System.Collections.Generic;

namespace Models.Reading
{
    public class CoverVm
    {
        public string Title { get; set; }

        public IEnumerable<AuthorCreditVm> Authors {get;set;}
    
        public DateTime? PublishDate { get; set; }         

        public IEnumerable<string> Genres { get; set; }
    }

    public class AuthorCreditVm
    {
        public string Name { get; set; }

        public int ProfileId { get; set; }

        public string ProfileUrl { get; set; }

    }

    //public class GenreCreditVm
    //{
    //    public string Name { get; set; }

    //}

}
