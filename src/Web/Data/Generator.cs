
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Reading;
using Web.Util;
using Models.Search;

namespace Models
{



    public static class Provider<T> where T : class
    {

        static Dictionary<System.Type, IProvider<T>> Registry = new Dictionary<System.Type, IProvider<T>>();


        public static T GenerateOne()
        {
            return Generate(1).First();
        }

        public static IEnumerable<T> Generate(int? count = null)
        {
            IProvider<T> instance = Registry[typeof(T)] as IProvider<T>;
            if (instance != null)
                return instance.GetMany(count);



            throw new ArgumentException("Type not registered " + typeof(T).Name);
        }

        static Provider()
        {
            RegisterTypes();
        }

        static void RegisterTypes()
        {
            Registry.Add(typeof(StoryVm), new StoryProvider() as IProvider<T>);
            Registry.Add(typeof(GenreVm), new GenreProvider() as IProvider<T>);
            Registry.Add(typeof(PreviewVm), new PreviewProvider() as IProvider<T>);
            Registry.Add(typeof(UserSearchVm), new UserSearchProvider() as IProvider<T>);
        }
    }

    public interface IProvider<T>
    {
        T Get();
        IEnumerable<T> GetMany(int? count);
    }

    public class StoryProvider : IProvider<StoryVm>
    {
        public StoryVm Get()
        {
            return GetMany(1).First();
        }

        public IEnumerable<StoryVm> GetMany(int? count = 10)
        {
            var genres = GenereDataRepository.GenreDefs.Keys.ToArray();

            Func<int,string[]> nGenres = (n) =>
            {
                var g = new List<string>();
                for(int i = 0; i < n; i++)
                {
                    var item = genres[Faker.RandomNumber.Next(genres.Length - 1)];
                    if (!g.Contains(item))
                        g.Add(item);
                }
                return g.ToArray();
            };

            return Builder<StoryVm>.CreateListOfSize(count.Value)
                    .All()
                    .With(t => t.Title = String.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(3, 9))).ToTitleCase())
                    .With(t => t.Description = String.Join(" ", Faker.Lorem.Sentences(Faker.RandomNumber.Next(2, 5))))
                    .With(t => t.Likes = Faker.RandomNumber.Next(10, 110))
                    .With(t => t.Dislikes = Faker.RandomNumber.Next(14, 40))
                    .With(t => t.Favorites = Faker.RandomNumber.Next(0, 14))
                    .With(t => t.Summary = String.Join("\r\n", Faker.Lorem.Paragraphs(2)))
                    .With(c => c.Genres = nGenres(Faker.RandomNumber.Next(1, 3)))
                    .With(c => c.Genre = String.Join(", ", c.Genres))
                    .With(t => t.LastUpdated = DateTime.Today.AddDays(-1 * Faker.RandomNumber.Next(1, 1000)))
                    .With(t => t.PublishDate = DateTime.Today.AddDays(-1 * Faker.RandomNumber.Next(1, 1000)))
                     .Build();
        }
    }

    public class GenreProvider : IProvider<GenreVm>
    {
        public GenreVm Get()
        {
            return GetMany().OrderBy(t => Faker.RandomNumber.Next()).First();
        }

        public IEnumerable<GenreVm> GetMany(int? count = null)
        {
            var genres = GenereDataRepository.GenreDefs.
                            Select((t, ix) =>
                           new GenreVm
                           {
                               Id = ix,
                               Name = t.Key,
                               Description = t.Value
                           });
            return genres;
        }
    }


    public static class GenereDataRepository
    {

        public static Dictionary<string, string> GenreDefs = new Dictionary<string, string>()
        {

            {"Crime/Detective", "fiction about a committed crime, how the criminal gets caught, and the repercussions of the crime"},
            {"Fable", "narration demonstrating a useful truth, especially in which animals speak as humans; legendary, supernatural tale"},
            {"Fairy Tale", "story about fairies or other magical creatures, usually for children"},
            {"Fanfiction", "fiction written by a fan of, and featuring characters from, a particular TV series, movie, etc."},
            {"Fantasy", "fiction with strange or otherworldly settings or characters; fiction which invites suspension of reality"},
            {"Fiction Narrative", "literary works whose content is produced by the imagination and is not necessarily based on fact"},
            {"Fiction in Verse", "full-length novels with plot, subplot(s), theme(s), major and minor characters, in which the narrative is presented in verse form(usually free verse)"},
            {"Folklore", "the songs, stories, myths, and proverbs of a people or \"folk\" as handed down by word of mouth"},
            {"Historical Fiction", "story with fictional characters and events in a historical setting"},
            {"Horror", "fiction in which events evoke a feeling of dread and sometimes fear in both the characters and the reader"},
            {"Humor", "Usually a fiction full of fun, fancy, and excitement, meant to entertain and sometimes cause intended laughter; but can be contained in all genres"},
            {"Legend", "story, sometimes of a national or folk hero, that has a basis in fact but also includes imaginative material"},
            {"Magical Realism", "story where magical or unreal elements play a natural part in an otherwise realistic environment"},
            {"Metafiction", "also known as romantic irony in the context of Romantic works of literature, uses self-reference to draw attention to itself as a work of art, while exposing the \"truth\" of a story"},
            {"Mystery", "this is fiction dealing with the solution of a crime or the unraveling of secrets"},
            {"Mythology", "legend or traditional narrative, often based in part on historical events, that reveals human behavior and natural phenomena by its symbolism; often pertaining to the actions of the gods"},
            {"Mythopoeia", "this is fiction where characters from religious mythology, traditional myths, folklores and history are recast into a re-imagined realm created by the author."},
            {"Realistic Fiction", "story that is true to life"},
            {"Science Fiction", "story based on impact of actual, imagined, or potential science, usually set in the future or on other planets"},
            {"Short Story", "fiction of such brevity that it supports no subplots"},
            {"Suspense/Thriller", "fiction about harm about to befall a person or group and the attempts made to evade the harm"},
            {"Tall Tale", "humorous story with blatant exaggerations, swaggering heroes who do the impossible with nonchalance"},
            {"Western", "set in the American Old West frontier and typically set in the late eighteenth to late nineteenth century"}

        };
    }



    public class UserSearchProvider : IProvider<UserSearchVm>
    {

        public UserSearchVm Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserSearchVm> GetMany(int? count)
        {
            var users = Builder<UserSearchVm>.CreateListOfSize(count.Value)
                .All()
                .With(t => t.Name = Faker.Internet.UserName())
                .With(t => t.LastActive = DateTime.Now.AddDays(-1 * Faker.RandomNumber.Next(1, 1000)))
                .With(t => t.JoinDate = t.LastActive.AddDays(-1 * Faker.RandomNumber.Next(1, 1000)))
                .With(t => t.Followers = Math.Max(0,Faker.RandomNumber.Next(-20,40)))
                .With(t => t.StoriesWrittenTo = Math.Max(0,Faker.RandomNumber.Next(-20,40)))
                .Build();

            return users;

        }
    }


}
