using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace MarkovChainer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<string> data = new List<string>();
            Console.WriteLine("Enter as many files as you want and leave the input blank when you are done.");
            while (true)
            {
                Console.Write(
                    "Please enter a file name: ");

                var input = Console.ReadLine();
                
                if (String.IsNullOrWhiteSpace(input))
                    if (data.Count > 0)
                        break;
                    else
                    {
                        Console.WriteLine("Please enter at least one file to train from.");
                        continue;
                    }

                try
                {
                    var file = File.ReadAllText(input);
                    data.Add(file);
                    Console.WriteLine("Successfully loaded file for training.");
                }
                catch
                {
                    Console.WriteLine("Invalid file or input. Please enter a valid file path.");
                }
            } 
            
            string dataString = String.Empty;
            foreach (var i in data)
                dataString += " " + i;

            Console.WriteLine(Markov(dataString, 2, 30000));

            Console.ReadKey();
        }

        // Learned from this exercise so far:
        //
        // - As opposed to using a position counter, use
        //   Linq's Skip(x) function on the for counter.
        // - As opposed to simply looping through and concatenating to a string,
        //   using Linq's Take(x) function.
        public static string Markov(string data, int keySize, int outputWords)
        {
            // key, expected next word with occurences
            Dictionary<string, Dictionary<string, int>> dict = new Dictionary<string, Dictionary<string, int>>();
            string[] words = data.Split(' ');

            int position = 0;
            for (int i = keySize - 1; i < words.Length - 1; i++)
            {
                string key = words[position];
                int target = position + keySize;
                for (int a = position + 1; a < target; a++)
                    key += " " + words[a];

                string next = words[position + keySize];
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new Dictionary<string, int>());
                    dict[key].Add(next, 1);
                }
                else
                {
                    var donct = dict[key];
                    if (!donct.ContainsKey(next))
                        donct.Add(next, 1);
                    else
                        donct[next]++;
                    dict[key] = donct;
                }
                
                position++;
            }
            
            Random rand = new Random();
            List<string> ret = new List<string>();
            var randPrefix = dict.Random();
            foreach (var i in randPrefix.Key.Split(' '))
                ret.Add(i);
            var prefix = randPrefix;

            for (int i = 0; i < outputWords; i++)
            {
                if (prefix.Value.Count <= 0)
                {
                    prefix = dict.Random();
                    foreach (var a in randPrefix.Key.Split(' '))
                        ret.Add(a);
                }
                
                Dictionary<double, string> probs = new Dictionary<double, string>();
                int total = prefix.Value.Sum(x => x.Value);
                double curTotal = 0;
                foreach (var a in prefix.Value)
                {
                    //Console.WriteLine($"Value added at curTotal: {curTotal}, of string: {a.Key}");
                    probs.Add(curTotal, a.Key);
                    curTotal += (double) a.Value / total;
                }

                var randy = rand.NextDouble();
                var suffix = probs[probs.Keys.Aggregate((x,y) => Math.Abs(x-randy) < Math.Abs(y-randy) ? x : y)];
                ret.Add(suffix);

                var newPrefix = String.Join(" ", ret.Skip(i + 1).Take(keySize).ToArray());
                if (!dict.ContainsKey(newPrefix))
                    prefix = dict.Random();
                else
                    prefix = new KeyValuePair<string, Dictionary<string, int>>(newPrefix, dict[newPrefix]);
            }

            return String.Join(" ", ret.ToArray());
        }
    }
    
    public static class DictExtensions
    {
        public static KeyValuePair<TKey, TValue> Random<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TKey> keys = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while(true)
            {
                var key = keys[rand.Next(size)];
                return new KeyValuePair<TKey,TValue>(key, dict[key]);
            }
        }
    }
    
}