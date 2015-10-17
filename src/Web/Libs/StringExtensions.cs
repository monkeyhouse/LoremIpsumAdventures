
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Web.Util
{


    public static class StringExtensions
    {
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        //http://stackoverflow.com/a/5796793/1778606
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if (partLength < 0)
            {
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));
            }

            for (var i = 0; i < s.Length; i += partLength)
            {
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }

    }

    public static class EnumExtensions
    {
        //http://stackoverflow.com/a/18855443/1778606
        public static Dictionary<int, string> GetEnumList<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new Exception("Type parameter should be of enum type");

            return Enum.GetValues(enumType).Cast<int>()
                       .ToDictionary(v => v, v => Enum.GetName(enumType, v).SplitCamelCase());
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new Exception("Type parameter should be of enum type");

            return Enum.GetValues(enumType).Cast<int>()
                   .Select(v => new SelectListItem() { Value = v.ToString(), Text = Enum.GetName(enumType, v).SplitCamelCase() });
        }
    }
}
