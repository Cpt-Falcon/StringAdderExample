//-----------------------------------------------------------------------
// <copyright file="StringAddTest.cs" company="Personal">
//     For demonstration purposes, copyright not applicable.
// </copyright>
// <author>Aubrey Russell</author>
// <date>10/9/2018</date>
//-----------------------------------------------------------------------

namespace UnitTestStringAdder
{
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
        }
    }
}
