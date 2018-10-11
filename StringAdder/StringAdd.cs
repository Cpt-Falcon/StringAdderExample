//-----------------------------------------------------------------------
// <copyright file="StringAdd.cs" company="Personal">
//     For demonstration purposes, copyright not applicable.
// </copyright>
// <author>Aubrey Russell</author>
// <date>10/9/2018</date>
//-----------------------------------------------------------------------

namespace StringAdder
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains methods and logic for adding a string together.
    /// </summary>
    public static class StringAdd
    {
        /// <summary>
        /// Use dynamic programming the improve performance for repetitive strings.
        /// </summary>
        private static Dictionary<string, int> numberCache = new Dictionary<string, int>();

        /// <summary>
        /// Adds a simple string.
        /// </summary>
        /// <param name="numbers">The numbers, in string format, that should be added.</param>
        /// <returns>Whether the program was successful.</returns>
        public static int Add(string numbers)
        {
            string originalNumbers = numbers;
            if (string.IsNullOrEmpty(numbers))
            {
                return 0;
            }

            // Try to get the cached value if possible to avoid the more costly processing.
            int cachedValue;
            if (numberCache.TryGetValue(numbers, out cachedValue))
            {
                return cachedValue;
            }

            // Use the string delimiter to store all delimiters along with default ones.
            List<string> delimiterList = new List<string>() { ",", "\n" };
            numbers = GetAndParseCustomDelimeters(numbers, delimiterList);
            string[] delimiters = delimiterList.ToArray();
            string[] splitNumbers = numbers.Split(delimiters, StringSplitOptions.None);
            int sum = CalculateSumFromSplitNumbers(splitNumbers);

            // Add to the cache if not found.
            numberCache.Add(originalNumbers, sum);
            return sum;
        }

        /// <summary>
        /// Clear the cache of its contents.
        /// </summary>
        public static void ClearCache()
        {
            numberCache.Clear();
        }

        /// <summary>
        /// Calculate the sum from the split numbers and throw an exception if it has negative numbers.
        /// </summary>
        /// <param name="splitNumbers">The split numbers.</param>
        /// <returns>The total sum.</returns>
        private static int CalculateSumFromSplitNumbers(string[] splitNumbers)
        {
            const int MaxNumber = 1000;
            int sum = 0;
            bool foundNegatives = false;
            string negativeNumbers = "negatives not allowed ";
            foreach (string number in splitNumbers)
            {
                int parsedNumber;
                if (int.TryParse(number, out parsedNumber))
                {
                    // Exclude negative numbers and change the found negative boolean to throw an exception later.
                    if (parsedNumber < 0)
                    {
                        negativeNumbers += parsedNumber.ToString() + " ";
                        foundNegatives = true;
                    }

                    // Don't add numbers to the sum if they are bigger than the max number.
                    if (parsedNumber <= MaxNumber)
                    {
                        sum += parsedNumber;
                    }
                }
            }

            if (foundNegatives)
            {
                throw new NegativesNotAllowed(negativeNumbers.TrimEnd());
            }

            return sum;
        }

        /// <summary>
        /// Gets, parses, and adds a delimiter to the list if its 
        /// </summary>
        /// <param name="numbers">The number string to parse.</param>
        /// <param name="delimiterList">The delimiter list to add delimiters to.</param>
        /// <returns>The remainder string with the delimiters removed.</returns>
        private static string GetAndParseCustomDelimeters(string numbers, List<string> delimiterList)
        {
            if (numbers.Length >= 2)
            {
                string checkCustomDelimeters = numbers.Substring(0, 2);
                if (checkCustomDelimeters == "//")
                {
                    // In this case we know we're going to be looking for custom delimeters.
                    // The regex captures the string from the slash to the new line character.
                    // Also better performance from using static pattern matching to reduce object instantiation.
                    string match = Regex.Match(numbers, "/(.*?)\\n").Value;

                    // We only want the contents after the two slashes and until right before the new line.
                    string contents = match.Substring(2, match.Length - 3);

                    // Match all instances containing square brackets to parse delimiters in square brackets.
                    MatchCollection matchCollection = Regex.Matches(contents, "\\[(.*?)\\]");
                    if (matchCollection.Count > 0)
                    {
                        char[] trimBrackets = { '[', ']' };
                        foreach (Match squareBracketMatch in matchCollection)
                        {
                            delimiterList.Add(squareBracketMatch.Value.Trim(trimBrackets));
                        }
                    }
                    else
                    {
                        delimiterList.Add(contents);
                    }

                    // Return the numbers string minus the delimiter initialization for better efficiency.
                    numbers = numbers.Substring(match.Length, numbers.Length - match.Length);
                } 
            }

            return numbers;
        }
    }
}
