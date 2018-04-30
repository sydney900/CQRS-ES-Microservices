using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;

namespace ClientProducer
{
    internal class NameGeneratorHelper
    {
        internal static IEnumerable<string> Generate(int number)
        {
            var personGenerator = new PersonNameGenerator();

            for (int i = 0; i < number; i++)
            {
                yield return personGenerator.GenerateRandomFirstAndLastName();
            }
        }
    }
}