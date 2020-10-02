using Ganss.XSS;
using System.Text.RegularExpressions;

namespace GDSHelpers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a string with any smart quotes replaced with normal quotes
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>The sanitized string</returns>
        public static string ReplaceSmartQuotes(this string s)
        {
            if (!string.IsNullOrEmpty(s))
                return s
                    .Replace('\u2018', '\'')        // Start single quote
                    .Replace('\u2019', '\'')        // End single quote
                    .Replace('\u201c', '\"')        // Start double quote
                    .Replace('\u201d', '\"');       // End double quote
            else
                return s;
        }

        /// <summary>
        /// Returns a string with any html removed
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>The sanitized string</returns>
        public static string StripHtml(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var htmlSanitizer = new HtmlSanitizer();
            str = htmlSanitizer.Sanitize(str);

            var result = Regex.Replace(str, @"<[^>]*>", string.Empty);
            return result;
        }

        /// <summary>
        /// Retrieve text from inside of html element like <label id='dd'>some text here</label>
        /// </summary>
        public static string GetTextInsideHtmlElement(this string htmlInput)
        {
            int tag1 = htmlInput.IndexOf(">");
            int tag2 = htmlInput.IndexOf("</");
            string rr = htmlInput.Substring(tag1 + 1, tag2 - tag1 - 1);
            return rr;
        }

    }
}
