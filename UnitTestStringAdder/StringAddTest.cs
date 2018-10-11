//-----------------------------------------------------------------------
// <copyright file="StringAddTest.cs" company="Personal">
//     For demonstration purposes, copyright not applicable.
// </copyright>
// <author>Aubrey Russell</author>
// <date>10/9/2018</date>
//-----------------------------------------------------------------------

namespace UnitTestStringAdder
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StringAdder;

    /// <summary>
    /// Test class that verifies the StringAdder class.
    /// </summary>
    [TestClass]
    public class StringAddTest
    {
        /// <summary>
        /// Tests whether an empty string can be added.
        /// </summary>
        [TestMethod]
        public void AddStringEmptyTest()
        {
            Assert.AreEqual(0, StringAdd.Add(string.Empty));
        }

        /// <summary>
        /// Tests whether a string with one number can be added.
        /// </summary>
        [TestMethod]
        public void AddOneNumberTest()
        {
            Assert.AreEqual(1, StringAdd.Add("1"));
            Assert.AreEqual(0, StringAdd.Add("0"));
            Assert.AreEqual(999, StringAdd.Add("999"));
        }

        /// <summary>
        /// Tests whether a string with two numbers can be added.
        /// </summary>
        [TestMethod]
        public void AddTwoNumbersTest()
        {
            Assert.AreEqual(3, StringAdd.Add("1,2"));
            Assert.AreEqual(999, StringAdd.Add("0,999"));
            Assert.AreEqual(22, StringAdd.Add("10,12"));
        }

        /// <summary>
        /// Tests whether a string with a N numbers will work.
        /// </summary>
        [TestMethod]
        public void AddManyNumbersTest()
        {
            string testString = string.Empty;
            int sum = 0;

            // Test with 10 numbers and calculate a sum of them as well to check.
            for (int i = 0; i < 10; i++)
            {
                sum += i;

                // Check if final number so it doesn't add comma at the end.
                if (i == 9)
                {
                    testString += i.ToString(); 
                }
                else
                {
                    testString += i.ToString() + ",";
                }
            }

            Assert.AreEqual(sum, StringAdd.Add(testString));

            // Reset the sum and string.
            testString = string.Empty;
            sum = 0;

            // Test with 100 numbers and calculate a sum of them as well to check.
            for (int i = 0; i < 100; i++)
            {
                sum += i;

                // Check if final number so it doesn't add comma at the end.
                if (i == 99)
                {
                    testString += i.ToString();
                }
                else
                {
                    testString += i.ToString() + ",";
                }
            }

            Assert.AreEqual(sum, StringAdd.Add(testString));
        }

        /// <summary>
        /// Tests whether a string with commas and new lines can be added.
        /// </summary>
        [TestMethod]
        public void AddNumbersWithNewLineCharactersTest()
        {
            Assert.AreEqual(6, StringAdd.Add("1\n2,3"));
            Assert.AreEqual(37, StringAdd.Add("5\n6\n2,5,9\n10"));
        }

        /// <summary>
        /// Tests whether a string with custom single character delimeters works.
        /// </summary>
        [TestMethod]
        public void AddNumbersWithDifferentSingleCharacterDelimetersTest()
        {
            Assert.AreEqual(3, StringAdd.Add("//;\n1;2"));
            Assert.AreEqual(23, StringAdd.Add("//z\n2z9\n5,7"));
            Assert.AreEqual(38, StringAdd.Add("//-\n2,9\n5,7-1-2-3-4-5"));
            
            // Test that the empty case doesn't cause an argument out of range exception.
            Assert.AreEqual(3, StringAdd.Add("//\n1,2"));
        }

        /// <summary>
        /// Tests whether adding negative numbers causes an exception and returns the appropriate error message.
        /// </summary>
        [TestMethod]
        public void AddNegativeNumbersExceptionTest()
        {
            var ex = Assert.ThrowsException<NegativesNotAllowed>(() => StringAdd.Add("10,9,8,44,12,-20,5,10,-2"));
            Assert.AreEqual("negatives not allowed -20 -2", ex.Message);

            ex = Assert.ThrowsException<NegativesNotAllowed>(() => StringAdd.Add("//;\n-1;-2,-3,-4,-5,-6,-7;-8,-9,-10"));
            Assert.AreEqual("negatives not allowed -1 -2 -3 -4 -5 -6 -7 -8 -9 -10", ex.Message);
        }

        /// <summary>
        /// Add Numbers greater than 1000 and test to make sure that larger numbers are not included in the sum.
        /// </summary>
        [TestMethod]
        public void AddNumbersGreaterThan1000Test()
        {
            Assert.AreEqual(3, StringAdd.Add("//;\n1;2000;2"));
            Assert.AreEqual(28, StringAdd.Add("//z\n2z9\n5,7,5,1001"));
            Assert.AreEqual(1038, StringAdd.Add("//-\n2,9\n5,7-1-2-3-4-5-1000"));
            Assert.AreEqual(2, StringAdd.Add("1001,2"));
        }

        /// <summary>
        /// Add Numbers that have a multiple length delimiter with the square brackets syntax. 
        /// </summary>
        [TestMethod]
        public void AddNumbersWithDelimitersOfAnylengthTest()
        {
            Assert.AreEqual(6, StringAdd.Add("//[***]\n1***2***3"));
            Assert.AreEqual(6, StringAdd.Add("//[;]\n1;2;3"));
            Assert.AreEqual(21, StringAdd.Add("//[;()]\n1;()2;()3,4,5\n6"));
        }

        /// <summary>
        /// Add a really long string to make sure performance is satisfactory. 
        /// We should be able to see any N^2 behavior or more readily in the amount of time it takes. 
        /// If its N^2 behavior or more this test should take probably more than a minute. If linear it should be under 1 second or less.
        /// </summary>
        [TestMethod]
        public void AddNumbersLongStringPerformanceTest()
        {
            const long MillisecondsPerSeconds = 1000; 
            string longStr = "&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&";
            Stopwatch stopwatch1 = Stopwatch.StartNew();

            Assert.AreEqual(68, StringAdd.Add("//[" + longStr + "]\n9" + longStr + "10" + longStr + "11" + longStr + "12" + longStr + "5" + longStr + "6" + longStr + "7" + longStr + "8"));
            stopwatch1.Stop();
            Assert.IsTrue(stopwatch1.ElapsedMilliseconds < MillisecondsPerSeconds, stopwatch1.ElapsedMilliseconds.ToString());
        }

        /// <summary>
        /// Add a string that has multiple delimiters, easy case with one character delimiters.
        /// </summary>
        [TestMethod]
        public void AddNumbersThatAllowMultipleDelimitersTest()
        {
            Assert.AreEqual(6, StringAdd.Add("//[*][%]\n1*2%3"));
            Assert.AreEqual(28, StringAdd.Add("//[*][%][&][a][b][c]\n1*2%3&4a5b6c7"));
        }

        /// <summary>
        /// Add a string that has multiple delimiters, hard case with multi character delimiters.
        /// </summary>
        [TestMethod]
        public void AddNumbersThatAllowMultipleDelimitersWithVaryingLengthsTest()
        {
            Assert.AreEqual(6, StringAdd.Add("//[***][%%%]\n1***2%%%3"));
            Assert.AreEqual(28, StringAdd.Add("//[**][%%%][&][aaaa][b][c-a]\n1**2%%%3&4aaaa5b6c-a7"));
        }

        /// <summary>
        /// Test duplicate delimiters edge case. 
        /// The result should be the same as if there were no duplicate to gracefully handle a user mistake.
        /// </summary>
        [TestMethod]
        public void AddNumbersWithDuplicateDelimiters()
        {
            Assert.AreEqual(15, StringAdd.Add("//[*][*][&][&]\n1*2*3&4&5"));
        }

        /// <summary>
        /// Test empty bracket delimiter. 
        /// The result should be the same as if there were no empty bracket to gracefully handle a user mistake.
        /// </summary>
        [TestMethod]
        public void AddNumbersWithEmptyBracket()
        {
            Assert.AreEqual(15, StringAdd.Add("//[*][][&][]\n1*2*3&4&5"));
        }

        /// <summary>
        /// Cleans up the cache at the end of the test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            StringAdd.ClearCache();
        }
    }
}
