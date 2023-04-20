using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

// reference:
// https://html-agility-pack.net/select-nodes
// https://www.w3schools.com/xml/xpath_syntax.asp

namespace SaiAkamCrawler;

public class HtmlParser
{
    public static Dictionary<string, int> ParseDocument(string body, HashSet<string> exclusionList)
    {
        Dictionary<string, int> wordCount = new();
        bool addWords = false;
        HtmlDocument content = new();
        content.LoadHtml(body);

        // get history subpage text: start from <h2> history
        HtmlNode outerContentNode = content.DocumentNode.SelectSingleNode("//div[@id='mw-content-text']");
        HtmlNode children = outerContentNode.SelectSingleNode("*");
        foreach (HtmlNode child in children.ChildNodes)
        {
            // accessing relevant info

            if (child.Name == "h2" && child.InnerText == "History")
            {
                addWords = true;
                ProcessWords(wordCount, child, exclusionList);
                continue;
            }
            if (addWords)
            {
                ProcessWords(wordCount, child, exclusionList);
            }

            if (child.Name == "h2")
            {
                break;
            }
            
        }

        return wordCount;
    }

    private static void ProcessWords(Dictionary<string, int> wordCount, HtmlNode childNode, HashSet<string> exclusionList)
    {
        string[] words = childNode.InnerText.Split(' ');
        foreach (string word in words)
        {
            string cleanedWord = word.Contains('.') ? word.Substring(0, word.IndexOf('.')) : word; // can better clean word for only letters via regex exp, and need to account for lowercacse
            if (!exclusionList.Contains(cleanedWord) && cleanedWord != " " && cleanedWord.Length > 0)
            {
                if (wordCount.ContainsKey(cleanedWord))
                {
                    wordCount[cleanedWord] = wordCount[cleanedWord] + 1;
                }
                else
                {
                    wordCount.Add(cleanedWord, 1);
                }
            }
        }
    }
}
