using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;

static class Program
{
    static char[] Separators = ("_.,<>{}[]()-+ \n\t\r\"\'`:;?!\\/|").ToCharArray();

    static string TimeId {
        get {
            System.Threading.Thread.Sleep(10);
            return String.Format("{0:x}", DateTime.UtcNow.Ticks);
        }
    }

    static List<string> SplitWords (string source)
    {
        var result = new List<string>();
        if (source == null || source.Length == 0) return result;
        result.AddRange(source.Split(Separators, StringSplitOptions.RemoveEmptyEntries));
        return result;
    } 

    static SortedDictionary<string, int> GroupByCount (
        IEnumerable<string> source, 
        bool ignoreCase = false)
    {
        var result = new SortedDictionary<string, int>();
        if (source == null) return result;
        foreach (string _item in source) {
            string item = ignoreCase ?_item.ToLower() : _item;
            int count = 0;
            result.TryGetValue(item, out count);
            result[item] = count + 1;
        }
        return result;
    }

    static bool OnlyOneSymbolRepeats(string word)
    {
        char? repeatedChar = null;
        for (int i=word.Length-1; i>0; i--) {
            for (int j=0; j<i; j++) {
                if (word[i] == word[j]) {
                    if (repeatedChar.HasValue && repeatedChar.Value != word[i])
                        return false;
                    repeatedChar = word[i];
                }
            }
        }
        // if repeatedChar has value, there was exactly one repeating symbol
        // otherwise, there were no repeating symbols
        return repeatedChar.HasValue;
    }

    static void Main (string[] args)
    {
        // Set up invariant culture
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        
        // Set up utf-8 output encoding
        var encoding = Console.OutputEncoding;
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length < 1) {
            Console.Write("Usage: <input_path>\n");
        }
        else if (File.Exists(args[0])) {
            // Read all text from file
            string text = File.ReadAllText(args[0]);

            // Get a list of words from text
            var words = SplitWords(text);
            Console.Write("Total: {0} words.\n", words.Count);

            // Find all words where only one symbol repeats
            var wordsQ = words.FindAll(OnlyOneSymbolRepeats);
            Console.Write("Only 1 symbol repeats: {0} words.\n", wordsQ.Count);
            foreach (var word in wordsQ) {
                Console.Write("{0}\n", word);
            }
            Console.Write("\n");

            // Group words by count
            var wordsWithCount = GroupByCount(words, ignoreCase: true);
            Console.Write("{0,-20} {1,6}\n", "Word", "Count");

            // Set up logger to write to Console and File simultaneously.
            var Log = new MultiStreamLogger (
                Console.Out, 
                new StreamWriter(args[0]+TimeId+".log"));

            // Write words in alphabetical order and their count
            foreach (var kvPair in wordsWithCount) {
                Log.Write("{0,-20} {1,6}\r\n", kvPair.Key, kvPair.Value);
            }
        }
        else {
            Console.Write("File '{0}' can not be found. \n", args[0]);
        }
        
        // Restore output encoding
        Console.OutputEncoding = encoding;
    }    
}
