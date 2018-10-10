//-----------------------------------------------------------------------
// <copyright file="NegativesNotAllowed.cs" company="Personal">
//     For demonstration purposes, copyright not applicable.
// </copyright>
// <author>Aubrey Russell</author>
// <date>10/9/2018</date>
//-----------------------------------------------------------------------

namespace StringAdder
{
    using System;

    /// <summary>
    /// Exception for not allowed negatives.
    /// </summary>
    public class NegativesNotAllowed : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativesNotAllowed"/> class.
        /// </summary>
        /// <param name="message">The custom message to specify containing the negative numbers that triggered the exception.</param>
        public NegativesNotAllowed(string message) : base(message)
        {
        }
    }
}
