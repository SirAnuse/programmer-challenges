using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace OccurenceCounter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string input = String.Empty;
            while (true)
            {
                Console.Write("Please enter a string to count characters in: ");
                input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Input string cannot be empty.");
                else
                    break;
            }
            
            Dictionary<char, int> dict = new Dictionary<char, int>();

            foreach (var ch in input)
            {
                if (!dict.ContainsKey(ch))
                    dict.Add(ch, 1);
                else
                    dict[ch]++;
            }
            
            Console.Write("Occurrence list: ");
            foreach (var ch in dict.OrderByDescending(x => x.Value))
                Console.Write($"'{ch.Key}': {ch.Value}, ");
            // do this to delete the last comma
            // hacky but whatever
            Console.Write("\b\b ");

            Console.ReadKey();
        }
    }
}