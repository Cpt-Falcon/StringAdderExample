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
    /// Adds the numbers in string together; numbers are added when separated by specified delimiters, for example: //[*][%]\n1*2%3
    /// This result of this example is 6.
    /// </summary>
    public static class StringAdd
    {
        /// <summary>
        /// Use dynamic programming to improve performance of repeat strings.
        /// </summary>
        private static Dictionary<string, int> numberCache = new Dictionary<string, int>();

        /// <summary>
        /// Adds adds a string together with specified or default delimiters.
        /// </summary>
        /// <param name="numbers">The numbers, contained in the string format, that should be added.</param>
        /// <returns>The sum of the numbers specified.</returns>
        public static int Add(string numbers)
        {
            string originalNumbers = numbers;

            // Microsoft recommends to use this method to validate strings.
            // Return 0 if null or empty since there are no numbers to add.
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

            // Use the string delimiter to store all delimiters along with the expected default ones.
            List<string> delimiterList = new List<string>() { ",", "\n" };
            numbers = ParseCustomDelimeters(numbers, delimiterList);
            string[] delimiters = delimiterList.ToArray();
            string[] splitNumbers = numbers.Split(delimiters, StringSplitOptions.None);

            int sum = CalculateSumFromSplitNumbers(splitNumbers);

            // Add to the cache if not found.
            numberCache.Add(originalNumbers, sum);
            return sum;
        }

        /// <summary>
        /// Clear the string repitition cache of its contents.
        /// </summary>
        public static void ClearCache()
        {
            numberCache.Clear();
        }

        /// <summary>
        /// Calculate the sum from the split numbers and throw an exception if there are negative numbers.
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
        /// Parse the delimiter initialization to determine contents.
        /// </summary>
        /// <param name="numbers">The number string to parse.</param>
        /// <param name="delimiterList">The delimiter list to add delimiters to.</param>
        /// <returns>The remainder string with the delimiters removed.</returns>
        private static string ParseCustomDelimeters(string numbers, List<string> delimiterList)
        {
            if (numbers.Length >= 2)
            {
                string checkCustomDelimeters = numbers.Substring(0, 2);
                if (checkCustomDelimeters == "//")
                {
                    // Look for custom delimiters when initialization slashes are included.
                    // The regex captures the string from the slash to the new line character.
                    // Also better performance from using static pattern matching to reduce object instantiation if this is used in a loop.
                    string delimiters = Regex.Match(numbers, "/(.*?)\\n").Value;

                    // If there is no match then we know this cannot be a valid delimiter initialization.
                    // Throw the exception to demonstrate 
                    if (delimiters == string.Empty)
                    {
                        throw new ArgumentException("The delimiter initialization is invalid.");
                    }

                    numbers = ParseDelimiterContents(delimiters, numbers, delimiterList);
                }
            }

            return numbers;
        }

        /// <summary>
        /// Parse the delimiters and add them to the list of delimiters.
        /// </summary>
        /// <param name="delimiters">The string that contains all of the delimiters.</param>
        /// <param name="numbers">The full string containing all the numbers and delimiters.</param>
        /// <param name="delimiterList">The delimiter list containing all of the delimiter strings that will be used.</param>
        /// <returns>The numbers string minues the delimiters</returns>
        private static string ParseDelimiterContents(string delimiters, string numbers, List<string> delimiterList)
        {
            // We only want the contents after the two slashes and until right before the new line.
            string contents = delimiters.Substring(2, delimiters.Length - 3);

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
            return numbers.Substring(delimiters.Length, numbers.Length - delimiters.Length);
        }
    }
}