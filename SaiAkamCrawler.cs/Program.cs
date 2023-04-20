using System;
using System.Runtime.CompilerServices;
using System.Collections;
using SaiAkamCrawler;

// get # words and exclusions
Console.WriteLine("Welcome to Sai Akam's web crawler!");
int runAttemptsLeft = 5;

object[] numWordsValidate = ProcessArgs.ProcessNumWords();
while (!Convert.ToBoolean(numWordsValidate[1]) && runAttemptsLeft > 0)
{
    numWordsValidate = ProcessArgs.ProcessNumWords();
    runAttemptsLeft--;
}
if (runAttemptsLeft == 0)
{
    Console.WriteLine("Error: incorrect arguments. Try again later");
    Environment.Exit(0);
}
runAttemptsLeft = 5;
object[] exclusionListValidate = ProcessArgs.ProcessExclusion();
while (!Convert.ToBoolean(exclusionListValidate[1]) && runAttemptsLeft > 0)
{
    exclusionListValidate = ProcessArgs.ProcessExclusion();
    runAttemptsLeft--;
}
if (runAttemptsLeft == 0)
{
    Console.WriteLine("Error: incorrect arguments. Try again later");
    Environment.Exit(0);
}

int numWords = Convert.ToInt32(numWordsValidate[0]);
string[] exclusionListArr = (string[])exclusionListValidate[0];
HashSet<string> exclusionList = new();
foreach (string exclusion in exclusionListArr)
{
    exclusionList.Add(exclusion);
}



// HTTP call to Microsoft Wikipedia subpage
runAttemptsLeft = 3;
HttpClient httpClient = new();
HttpResponseMessage response = await httpClient.GetAsync("https://en.wikipedia.org/wiki/Microsoft#History");
string body;
while (!response.IsSuccessStatusCode && runAttemptsLeft > 0)
{
    response = await httpClient.GetAsync("https://en.wikipedia.org/wiki/Microsoft#History");
    runAttemptsLeft--;
}
if (response.IsSuccessStatusCode && response.Content != null)
{
    body = await response.Content.ReadAsStringAsync();
}
else
{
    body = null;
    Console.WriteLine("Error: unable to connect to the input URL. Check your internet connection or try again later");
    Environment.Exit(0);
}



// parse html
Dictionary<string, int> mostCommonWordsCounts = HtmlParser.ParseDocument(body, exclusionList);

if (mostCommonWordsCounts.Count == 0 || mostCommonWordsCounts == null)
{
    Console.WriteLine("No search words found in the document.");
}
else
{
    // sort dictionary
    var sortedCommonWordsDesc = from entry in mostCommonWordsCounts orderby entry.Value descending select entry;
    Console.WriteLine($"The top {numWords} commonly occurring words:");
    foreach (KeyValuePair<string, int> wordCount in sortedCommonWordsDesc)
    {
        if (numWords > 0)
        {
            Console.Write(wordCount.Key);
            Console.Write(" | ");
            Console.Write(wordCount.Value);
            Console.WriteLine();
            Console.WriteLine("--------");
        }
        else
        {
            break;
        }
        numWords--;
    }
}

