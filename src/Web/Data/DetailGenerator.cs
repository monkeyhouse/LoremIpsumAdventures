
using FizzWare.NBuilder;
using Microsoft.AspNet.Mvc;
using Models.Meta;
using Models.Reading;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Web.Util;
using Models.Profile;

namespace Models
{

    public static class DetailProvider<T> where T : class
    {
        static Dictionary<System.Type, IDetailProvider<T>> Registry = new Dictionary<System.Type, IDetailProvider<T>>();

        public static T Generate()
        {
            IDetailProvider<T> instance = Registry[typeof(T)] as IDetailProvider<T>;
            if (instance != null)
                return instance.Get();

            throw new ArgumentException("Type not registered " + typeof(T).Name);
        }

        static DetailProvider()
        {
            RegisterTypes();
        }

        static void RegisterTypes()
        {
            Registry.Add(typeof(PreviewVm), new PreviewProvider() as IDetailProvider<T>);
            Registry.Add(typeof(CoverVm), new CoverProvider() as IDetailProvider<T>);
            Registry.Add(typeof(PageVm), new PageProvider() as IDetailProvider<T>);
            Registry.Add(typeof(JacketVm), new JacketProvider() as IDetailProvider<T>);
            Registry.Add(typeof(PlayerStoryStatsVm), new PlayerStoryStatsProvider() as IDetailProvider<T>);
            Registry.Add(typeof(UserSummaryVm), new UserSummaryProvider() as IDetailProvider<T>);
            Registry.Add(typeof(StoriesStatsVm), new ProfileStoryStatsProvider() as IDetailProvider<T>);
        }
    }

    public interface IDetailProvider<T>
    {
        T Get();
    }


    public class PreviewProvider : IDetailProvider<PreviewVm>
    {
        public PreviewVm Get()
        {

            var story = Builder<PreviewVm>.CreateNew().Build();
            story.Summary = String.Join("\r\n", Faker.Lorem.Paragraphs(2));
            story.Title = String.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(3,10))).ToTitleCase();
            story.Pages = new List<PreviewPageVm>();
            for (var i = 1; i <= 3; i++)
            {
                var page = new PreviewPageVm();
                page.PageNumber = i;
                page.Body = String.Join("\r\n", Faker.Lorem.Paragraphs(Faker.RandomNumber.Next(1, 4)));
                page.Actions = Faker.Lorem.Sentences(Faker.RandomNumber.Next(2, 4)).ToList();

                story.Pages.Add(page);
            }


            return story;
        }
    }


    public class CoverProvider : IDetailProvider<CoverVm>
    {
        public CoverVm Get()
        {
            var cover = new CoverVm();

            cover.Title = String.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(4, 10))).ToTitleCase();
            cover.Authors = GetAuthors(Faker.RandomNumber.Next(1, 4));
            cover.PublishDate = DateTime.Today.AddDays(-1 * Faker.RandomNumber.Next(1, 1000));
            cover.Genres = Provider<GenreVm>.Generate().OrderBy(r => Faker.RandomNumber.Next()).Take(Faker.RandomNumber.Next(1, 3)).Select(t => t.Name);

            return cover;
        }

        private IEnumerable<AuthorCreditVm> GetAuthors(int count = 1)
        {
            var authors = new List<AuthorCreditVm>();

            for (var i = 0; i < count; i++) {
                authors.Add(new AuthorCreditVm
                {
                    Name = Faker.Internet.UserName(),
                    ProfileId = Faker.RandomNumber.Next(1000, 10000)
                });
            }
            return authors;
        }
    }

    public class PageProvider : IDetailProvider<PageVm>
    {
        public PageVm Get()
        {
            var page = new PageVm();
            page.PageId = Faker.RandomNumber.Next();
            page.PageNumber = Faker.RandomNumber.Next(1, 100);
            page.Text = String.Join("\r\n", Faker.Lorem.Paragraphs(Faker.RandomNumber.Next(2, 5)));
            page.Actions = GetActions(Faker.RandomNumber.Next(1, 4));
            return page;
        }

        private List<ActionVm> GetActions(int count)
        {
            var actions = new List<ActionVm>();

            for (var i = 0; i < count; i++)
            {
                actions.Add(new ActionVm()
                {
                    Text = Faker.Lorem.Sentence(5),
                    PageId = Faker.RandomNumber.Next(1, 1000)
                });
            }

            return actions;
        }
    }

    public class JacketProvider : IDetailProvider<JacketVm>
    {
        public JacketVm Get()
        {
            var jacket = new JacketVm();
            jacket.SectionTitle = String.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(2, 4))).ToTitleCase();
            jacket.SectionText = String.Join(Environment.NewLine, Faker.Lorem.Paragraphs(Faker.RandomNumber.Next(2, 4)));
            return jacket;
        }
    }

    public class PlayerStoryStatsProvider : IDetailProvider<PlayerStoryStatsVm>
    {
        public PlayerStoryStatsVm Get()
        {
            var stats = new PlayerStoryStatsVm();
            stats.MinutesPlayed = Faker.RandomNumber.Next(1, 25);
            stats.Plays = Faker.RandomNumber.Next(2, 5);
            stats.EndingsAvailable = Faker.RandomNumber.Next(2, 4);
            stats.EndingCompleted = Faker.RandomNumber.Next(0, stats.EndingsAvailable);
            return stats;
        }
    }

    public class UserSummaryProvider : IDetailProvider<UserSummaryVm>
    {
        public UserSummaryVm Get()
        {
            var summary = new UserSummaryVm();

            summary.Name = Faker.Internet.UserName();
            summary.LastActive = ProviderMethods.GetRecentDate();
            summary.JoinDate = summary.LastActive.AddDays(-Faker.RandomNumber.Next(1, 1000));

            return summary;
        }
    }

    public class ProfileStoryStatsProvider : IDetailProvider<StoriesStatsVm>
    {
        public StoriesStatsVm Get()
        {
            var stats = new StoriesStatsVm();
            stats.Favs = Faker.RandomNumber.Next(1, 40);
            stats.Likes = Faker.RandomNumber.Next(1, 100);
            stats.InProgress = ProviderMethods.GetZeroOrNumber(10);
            stats.Published = ProviderMethods.GetZeroOrNumber(40);
            return stats;
        }
    }


    public static class ProviderMethods{
        public static DateTime GetRecentDate()
        {
            return DateTime.Today.AddDays(-1 * Faker.RandomNumber.Next(1, 1000));
        }

        public static int GetZeroOrNumber( int upTo = 40, float zeroChance = .2f)
        {
            if (Faker.RandomNumber.NextDouble() < zeroChance || upTo == 0)
                return 0;

            return Faker.RandomNumber.Next(1, upTo);
        }
    }

}
