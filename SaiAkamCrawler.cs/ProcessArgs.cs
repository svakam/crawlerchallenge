using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaiAkamCrawler;

public class ProcessArgs
{
    public static object[] ProcessNumWords()
    {
        // take user input for number of words
        Console.WriteLine("Provide the number of most common words to search for:");
        string numWordsArg = Console.ReadLine();


        // validate and return result
        object[] validResult;
        if (int.TryParse(numWordsArg, out int num))
        {
            Console.WriteLine($"You asked to return {num} words.");
            validResult = new object[] { num, true };
        }
        else if (numWordsArg == null || numWordsArg.Length == 0) // default 10
        {
            Console.WriteLine("No argument provided for number of words. Default is 10");
            validResult = new object[] { 10, true };
        }
        else
        {
            Console.WriteLine($"Provided the following invalid input for number of words: {numWordsArg}. Try again");
            validResult = new object[] { -1, false };
        }
        return validResult;
    }

    public static object[] ProcessExclusion()
    {
        // take user input for exclusion list
        Console.WriteLine("Provide a list of words to exclude from the most common word search:");
        string exclusionsArg = Console.ReadLine();

        // validate and return
        object[] validResult;
        string[] exclusionList;
        if (exclusionsArg != null && exclusionsArg.Length > 0)
        {
            exclusionList = exclusionsArg.Split(',');

            // validate list
            foreach (string exclusion in exclusionList)
            {
                if (exclusion.GetType() != typeof(string))
                {
                    Console.WriteLine($"Error: received the following invalid input for exclusion: {exclusion}. Try again\n");
                    validResult = new object[] { "", false };
                }
            }
            validResult = new object[] { exclusionList, true };
        }
        else
        {
            Console.WriteLine($"Error: did not receive an exclusion list. Try again\n");
            validResult = new object[] { "", false };
        }
        return validResult;
    }
}