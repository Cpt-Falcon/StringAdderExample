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
    }
}
