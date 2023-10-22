using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace WordAnalyzerThreaded
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("|| Enter the path of the text file of your selected book:");
            var bookFile = Console.ReadLine();

            if (File.Exists(bookFile))
            {
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                
                StreamReader book = new StreamReader(bookFile);
                string text = book.ReadToEnd();
                string[] words = GetBookWords(text);
                book.Close();

                Thread totalWordsThread = new Thread(delegate ()
                {
                    Console.WriteLine($"Total words: " + words.Length + " words");
                    Console.WriteLine();
                });
                Thread shortestWordThread = new Thread(delegate ()
                {
                    Console.WriteLine($"Shortest word: " + GetShortestWord(words));
                    Console.WriteLine();
                });
                Thread longestWordThread = new Thread(delegate ()
                {
                    Console.WriteLine($"Longest word: " + GetLongestWord(words));
                    Console.WriteLine();
                });
                Thread averageWordLengthThread = new Thread(delegate ()
                {
                    float averageWordLength = GetAverageLength(words);
                    Console.WriteLine($"Average word length: {averageWordLength:F2} characters");
                    Console.WriteLine();
                });
                Thread mostFrequentWordsThread = new Thread(delegate ()
                {
                    string[] mostFrequentWords = GetMostFrequentWords(words);
                    Console.WriteLine("Most frequent words:");
                    for (int i = 0; i < mostFrequentWords.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. " + mostFrequentWords[i]);
                    }
                    Console.WriteLine();
                });
                Thread leastFrequentWordsThread = new Thread(delegate ()
                {
                    string[] leastFrequentWords = GetLeastFrequentWords(words);
                    Console.WriteLine("Least frequent words:");
                    for (int i = 0; i < leastFrequentWords.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. " + leastFrequentWords[i]);
                    }
                    Console.WriteLine();
                });

                totalWordsThread.Start();
                shortestWordThread.Start();
                longestWordThread.Start();
                averageWordLengthThread.Start();
                mostFrequentWordsThread.Start();
                leastFrequentWordsThread.Start();

                sw.Stop();

                Console.WriteLine();
                Console.WriteLine($"|| Threaded version execution time: {sw.ElapsedMilliseconds} ms.");
            }
            else
            {
                Console.WriteLine("|| Book file invalid or not found.");
            }
        }

        public static string[] GetBookWords(string text)
        {
            string[] words = Regex.Split(text, @"\W+");
            List<string> wordsList = new List<string>();
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length < 3)
                {
                    continue;
                }
                wordsList.Add(words[i]);
            }
            return wordsList.ToArray();
        }

        public static string GetShortestWord(string[] words)
        {
            string shortestWord = words[0];

            foreach (var word in words)
            {
                if (shortestWord.Length > word.Length)
                {
                    shortestWord = word;
                }
            }
            return shortestWord;
        }

        public static string GetLongestWord(string[] words)
        {
            string longestWord = words[0];

            foreach (var word in words)
            {
                if (longestWord.Length < word.Length)
                {
                    longestWord = word;
                }
            }
            return longestWord;
        }

        public static float GetAverageLength(string[] words)
        {
            int wordLength = 0;

            foreach (var word in words)
            {
                wordLength += word.Length;
            }
            return (float)wordLength / words.Length;
        }

        public static string[] GetMostFrequentWords(string[] words)
        {
            int count = 0;
            string[] mostFrequentWords = new string[5];
            var wordFrequency = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency[word] = 1;
                }
            }

            foreach (var word in wordFrequency)
            {
                if (mostFrequentWords[count] == null)
                {
                    mostFrequentWords[count] = word.Key;
                }
                if (count < 4)
                {
                    count++;
                }
                else
                {
                    for (int i = 0; i < mostFrequentWords.Length; i++)
                    {
                        if (wordFrequency[mostFrequentWords[i]] < word.Value)
                        {
                            mostFrequentWords[i] = word.Key;
                            break;
                        }
                    }
                }
            }
            return mostFrequentWords;
        }

        public static string[] GetLeastFrequentWords(string[] words)
        {
            int count = 0;
            string[] leastFrequentWords = new string[5];
            var wordFrequency = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency[word] = 1;
                }
            }

            foreach (var word in wordFrequency)
            {
                if (leastFrequentWords[count] == null)
                {
                    leastFrequentWords[count] = word.Key;
                }
                if (count < 4)
                {
                    count++;
                }
                else
                {
                    for (int i = 0; i < leastFrequentWords.Length; i++)
                    {
                        if (wordFrequency[leastFrequentWords[i]] > word.Value)
                        {
                            leastFrequentWords[i] = word.Key;
                            break;
                        }
                    }
                }
            }
            return leastFrequentWords;
        }
    }
}
