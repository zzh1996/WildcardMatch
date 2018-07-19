using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildcardMatch
{
    public class Program
    {
        // only for console application test
        // now the project is converted to a class library, so this is useless now
        /* static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Pattern:");
                string pattern = Console.ReadLine();
                Console.WriteLine("Value:");
                string value = Console.ReadLine();
                Console.WriteLine(Match(pattern, value).ToString());
            }
        } */

        public static bool Match(string pattern, string value)
        {
            const string star = "star";
            const string question = "question";

            // split tokens in the pattern into a list
            List<string> tokens = new List<string>();
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '*')
                    tokens.Add(star);
                else if (pattern[i] == '?')
                    tokens.Add(question);
                else if (pattern[i] == '+')
                {
                    // '+' is euqual to '?*'
                    tokens.Add(question);
                    tokens.Add(star);
                }
                else if (pattern[i] == '\\')
                {
                    if (i == pattern.Length - 1)
                    {
                        // '\' is the last char in pattern
                        throw new ArgumentException();
                    }
                    else
                    {
                        if (pattern[i + 1] == '*' || pattern[i + 1] == '?' || pattern[i + 1] == '+' || pattern[i + 1] == '\\')
                        {
                            tokens.Add(pattern[i + 1].ToString());
                            i++;
                        }
                        else
                        {
                            // '\' followed by an invalid char
                            throw new ArgumentException();
                        }
                    }
                }
                else
                {
                    tokens.Add(pattern[i].ToString());
                }
            }

            bool[,] match = new bool[tokens.Count + 1, value.Length + 1]; // 2D array for DP
            match[0, 0] = true; // "" matches ""
            for (int p = 0; p < tokens.Count; p++)
            {
                string token = tokens[p];
                // pattern[0..p] matches "" <-> pattern[0..p-1] matches "" and pattern[p] is '*'
                match[p + 1, 0] = token == star && match[p, 0];
                for (int s = 0; s < value.Length; s++)
                {
                    string c = value[s].ToString();
                    if (token == star)
                    {
                        // pattern[0..p] matches string[0..s] <-> pattern[0..p-1] matches string[0..s] or pattern[0..p] matches string[0..s-1]
                        match[p + 1, s + 1] = match[p, s + 1] || match[p + 1, s];
                    }
                    else
                    {
                        // pattern[0..p] matches string[0..s] <-> pattern[0..p-1] matches string[0..s-1] and pattern[p] matches string[s]
                        match[p + 1, s + 1] = match[p, s] && (token == question || token == c);
                    }
                }
            }

            return match[tokens.Count, value.Length];
        }
    }
}
