
namespace Models.Search
{
  

    public enum SearchSortType
    {
        //all
        BestMatch = 0,

        //stories
        RecentlyPublished = 1,      //Publish Date, Desc
        LeastRecentlyPublished = 2, //Publish Date, Asc
        MostLikes = 3,              //Likes, Desc
        FewestLikes = 4,            //Likes, Asc
        MostFavorites = 5,          //Favs, Desc
        FewestFavorites = 6,        //Favs, Asc


        //users
        RecentlyJoined = 7,         //Join Date, Desc
        LeastRecentlyJoined = 8,    //Join Date, Asc
        MostFollowers = 9,          //Followers, Desc
        LeastFollowers = 10         //Followers, Asc
    }


}