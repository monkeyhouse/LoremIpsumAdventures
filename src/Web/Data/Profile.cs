using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeData
{
    public static class Profile
    {
        public static bool IsLoggedIn { get; set; }

        public static string MyFavoriteGenres = "";

        public static List<GenreVm> MyFavoriteGenreVms { get; set; }

        static Profile()
        {
            IsLoggedIn = true;

            MyFavoriteGenreVms = Provider<GenreVm>.Generate()
                                .OrderBy(t => Faker.RandomNumber.Next())
                                .Take( Faker.RandomNumber.Next(4,10)).ToList();

            SetGenreString();            
        }

        private static void SetGenreString()
        {
            MyFavoriteGenres = String.Join(",", MyFavoriteGenreVms.Select(t => t.Name));
        }


        public static void AddGenre(int genreId )
        {
            ToggleFavoriteGenre(genreId, ActionEnum.Add);
        }

        public static void RemoveGenre(int genreId)
        {
            ToggleFavoriteGenre(genreId, ActionEnum.Remove);
        }

        /// <summary>
        /// Add or remove genre from profile favorites
        /// </summary>
        /// 
        private static void ToggleFavoriteGenre( int genreId, ActionEnum action = 0)
        {
            if (action == 0)
                throw new ArgumentOutOfRangeException(nameof(action), $" {typeof(ActionEnum)} {nameof(action)} is undefined " );

            if ( action == ActionEnum.Remove)
            {
                var item = MyFavoriteGenreVms.First(t => t.Id == genreId);
                MyFavoriteGenreVms.Remove(item);
                SetGenreString();
            }

            if (action == ActionEnum.Add)
            {
                var allGenres = Provider<GenreVm>.Generate();
                var item = allGenres.First(t => t.Id == genreId);
                if ( ! MyFavoriteGenreVms.Any(t => t.Id == genreId) )
                    MyFavoriteGenreVms.Add(item);

                SetGenreString();
            }

        }

        public enum ActionEnum
        {
            Undefined = 0,
            Add = 1,
            Remove = -1
        }
    }
}
