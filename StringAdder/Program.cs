//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Personal">
//     For demonstration purposes, copyright not applicable.
// </copyright>
// <author>Aubrey Russell</author>
// <date>10/9/2018</date>
//-----------------------------------------------------------------------

namespace StringAdder
{
    using System;

    /// <summary>
    /// The start of the console application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args">The main method arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Add a string with the following format: //[delim1][delim2]\\n   ---   //[*][%]\\n1*2%3");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    return;
                }
                else
                {
                    try
                    {
                        // Allows users to type new lines literally without triggering the console read line so that the example work as specified.
                        input = input.Replace("\\n", "\n");
                        Console.WriteLine(StringAdd.Add(input));
                    }
                    catch (NegativesNotAllowed e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
