using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAndFollowSet
{
    class Program
    {
        static Dictionary<char, HashSet<char>> firstSets = new Dictionary<char, HashSet<char>>();
        static Dictionary<char, HashSet<char>> followSets = new Dictionary<char, HashSet<char>>();
        static Dictionary<char, List<string>> productions = new Dictionary<char, List<string>>();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of productions:");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the productions in the format A->xyz (use '|' for epsilon):");
            for (int i = 0; i < n; i++)
            {
                string production = Console.ReadLine();
                char nonTerminal = production[0];
                string rhs = production.Substring(3); // Extracting right-hand side of the production
                if (!productions.ContainsKey(nonTerminal))
                {
                    productions[nonTerminal] = new List<string>();
                }
                productions[nonTerminal].Add(rhs);
            }

            ComputeFirstSets();
            ComputeFollowSets();

            Console.WriteLine("\nFirst Sets:");
            foreach (var kvp in firstSets)
            {
                Console.WriteLine($"First({kvp.Key}) = {{ {string.Join(", ", kvp.Value)} }}");
            }

            Console.WriteLine("\nFollow Sets:");
            foreach (var kvp in followSets)
            {
                Console.WriteLine($"Follow({kvp.Key}) = {{ {string.Join(", ", kvp.Value)} }}");
            }
        }

        static void ComputeFirstSets()
        {
            foreach (var kvp in productions)
            {
                char nonTerminal = kvp.Key;
                ComputeFirstSet(nonTerminal);
            }
        }

        static void ComputeFirstSet(char nonTerminal)
        {
            if (!firstSets.ContainsKey(nonTerminal))
            {
                firstSets[nonTerminal] = new HashSet<char>();
            }

            foreach (string production in productions[nonTerminal])
            {
                char symbol = production[0];
                if (char.IsUpper(symbol))
                {
                    ComputeFirstSet(symbol);
                    foreach (char terminal in firstSets[symbol])
                    {
                        firstSets[nonTerminal].Add(terminal);
                    }
                }
                else
                {
                    firstSets[nonTerminal].Add(symbol);
                }
            }
        }

        static void ComputeFollowSets()
        {
            foreach (var kvp in productions)
            {
                char nonTerminal = kvp.Key;
                ComputeFollowSet(nonTerminal);
            }
        }

        static void ComputeFollowSet(char nonTerminal)
        {
            if (!followSets.ContainsKey(nonTerminal))
            {
                followSets[nonTerminal] = new HashSet<char>();
            }

            if (nonTerminal == productions.Keys.First()) // If it's the start symbol, add '$' to its follow set
            {
                followSets[nonTerminal].Add('$');
            }

            foreach (var kvp in productions)
            {
                char key = kvp.Key;
                foreach (string production in kvp.Value)
                {
                    int index = production.IndexOf(nonTerminal);
                    while (index != -1 && index < production.Length - 1)
                    {
                        char next = production[index + 1];
                        if (char.IsUpper(next)) // If next is a non-terminal
                        {
                            foreach (char terminal in firstSets[next])
                            {
                                followSets[nonTerminal].Add(terminal);
                            }
                            if (firstSets[next].Contains('|')) // If next can derive epsilon
                            {
                                foreach (char terminal in followSets[key])
                                {
                                    followSets[nonTerminal].Add(terminal);
                                }
                            }
                        }
                        else // If next is a terminal
                        {
                            followSets[nonTerminal].Add(next);
                        }
                        index = production.IndexOf(nonTerminal, index + 1);
                    }
                }
            }
        }
    }
}
