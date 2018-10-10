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
        /// Adds a simple string.
        /// </summary>
        /// <param name="numbers">The numbers, in string format, that should be added.</param>
        /// <returns>Whether the program was successful.</returns>
        public static int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
            {
                return 0;
            }

            // Use the string delimiter to store all delimiters along with default ones.
            List<string> delimiterList = new List<string>() { ",", "\n" };
            numbers = GetAndParseCustomDelimeters(numbers, delimiterList);
            int sum = 0;
            string[] delimiters = delimiterList.ToArray();
            string[] splitNumbers = numbers.Split(delimiters, StringSplitOptions.None);
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

                    sum += parsedNumber; 
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
                    delimiterList.Add(contents);
                    numbers = numbers.Substring(match.Length, numbers.Length - match.Length);
                } 
            }

            return numbers;
        }
    }
}
