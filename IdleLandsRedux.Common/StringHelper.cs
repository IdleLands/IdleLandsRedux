﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace IdleLandsRedux.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Sanitizes the string.
        /// </summary>
        /// <returns>Sanitized string.</returns>
        /// <param name="str">Input to be sanitized.</param>
        /// <param name="punctuation">If set to <c>true</c>, allows for punctuation. Otherwise it is also sanitized.</param>
        public static string SanitizeString(string str, bool punctuation = false)
        {
            if (!punctuation)
                return Regex.Replace(str, "[^a-zA-Z0-9 ]", string.Empty, RegexOptions.Compiled);
            return Regex.Replace(str, "[^a-zA-Z0-9_,.;?! ]", string.Empty, RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets the gender pronoun.
        /// </summary>
        /// <returns>The gender pronoun.</returns>
        /// <param name="gender">Gender. e.g. 'Male' or 'Female'</param>
        /// <param name="input">Input. e.g. '%hisher' or 'she'</param>
        public static string GetGenderPronoun(string gender, string input)
        {
            if (string.IsNullOrEmpty(gender))
            {
                throw new ArgumentNullException(nameof(gender));
            }

            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            gender = gender.ToLower();
            input = input.ToLower();
            switch (input)
            {
                case "%hisher":
                    if (gender == "male")
                        return "his";
                    else if (gender == "female")
                        return "her";
                    else
                        return "their";
                case "%hishers":
                    if (gender == "male")
                        return "his";
                    else if (gender == "female")
                        return "hers";
                    else
                        return "theirs";
                case "%himher":
                    if (gender == "male")
                        return "him";
                    else if (gender == "female")
                        return "her";
                    else
                        return "them";
                case "%heshe":
                    if (gender == "male")
                        return "he";
                    else if (gender == "female")
                        return "she";
                    else
                        return "they";
                case "%she":
                    if (gender == "male")
                        return "he";
                    else if (gender == "female")
                        return "she";
                    else
                        return "they";
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// Contains any of the given needles.
        /// </summary>
        /// <returns><c>true</c>, if any was contained, <c>false</c> otherwise.</returns>
        /// <param name="haystack">Haystack.</param>
        /// <param name="needles">Needles.</param>
        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            if (string.IsNullOrEmpty(haystack))
                return false;

            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }

        public static string Capitalize(this string @string, int offset = 0)
        {
            if (string.IsNullOrEmpty(@string))
            {
                throw new ArgumentNullException(nameof(@string));
            }

            return @string.Substring(0, offset) + Char.ToUpper(@string[offset]).ToString() + @string.Substring(offset + 1);
        }

        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule", Justification = "Gendarme is blind. Parameter name is used correctly.")]
        public static StringBuilder ReplaceGenderPronoun(this StringBuilder builder, string gender, string input)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(gender))
            {
                throw new ArgumentNullException(nameof(gender));
            }

            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Length < 2)
            {
                throw new ArgumentOutOfRangeException("input length is too short. Minimum of 2.");
            }

            string genderPronoun = StringHelper.GetGenderPronoun(gender, input);

            if (Char.IsUpper(input[1]))
                genderPronoun = genderPronoun.Capitalize();

            return builder.Replace(input, genderPronoun);
        }
    }
}

