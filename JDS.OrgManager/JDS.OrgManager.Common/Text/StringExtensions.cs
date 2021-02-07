using Humanizer;
using System;
using System.Text.RegularExpressions;

namespace JDS.OrgManager.Common.Text
{
    public static class StringExtensions
    {
        #region Private Fields

        private static readonly Regex digitsRegex = new Regex(@"\d", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex nonDigitsRegex = new Regex(@"\D", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex nonWordRegex = new Regex(@"[\s\W]+", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex whitespaceAndDashesRegex = new Regex(@"[\s\-]+", RegexOptions.Compiled | RegexOptions.Singleline);

        #endregion

        #region Public Methods

        public static string ReplaceDigits(this string text, string replacement) => digitsRegex.Replace(text ?? "", replacement);

        public static string Sluggify(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            var slug = str.Humanize(LetterCasing.Title);
            slug = whitespaceAndDashesRegex.Replace(slug, "-").Trim('-').ToLower();
            return slug;
        }

        public static string StripNonDigits(this string text) => nonDigitsRegex.Replace(text ?? "", "");

        public static string StripNonWord(this string str) => nonWordRegex.Replace(str ?? "", "");

        #endregion
    }
}