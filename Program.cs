using HtmlAgilityPack;
using static System.Net.WebRequestMethods;

namespace SimpleWebScraper
{
    class Program
    {


        static void Main(string[] args)
        {
            //string url = "https://www.wendroffcpa.com/wendroff-associates-featured-in-arlington-magazine/";

            Console.WriteLine("Please enter your URL: ");
            string? url = Console.ReadLine();
            if (url != null)
            {
                Start(url);
            }
            
        }

        static void Start(string url)
        {
            List<string> wordlist = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                var sourceCode = client.GetStringAsync(url).Result;
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(sourceCode);

                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@id='stickyContainer']"))
                {
                    string content = node.InnerText;
                    var words = content.ToLower().Split();

                    foreach (string word in words)
                    {
                        wordlist.Add(word);
                    }
                }
            }

            CleanWordList(wordlist);
        }
        static void CleanWordList(List<string> wordlist)
        {
            List<string> cleanList = new List<string>();
            string symbols = "!@#$%^&*()_-+={[}]|;:\",.<>/?";

            foreach (string word in wordlist)
            {
                string cleanedWord = word;
                foreach (char symbol in symbols)
                {
                    cleanedWord = cleanedWord.Replace(symbol.ToString(), "");
                }

                if (!string.IsNullOrEmpty(cleanedWord))
                {
                    cleanList.Add(cleanedWord);
                }
            }

            CreateDictionary(cleanList);
        }

        static void CreateDictionary(List<string> cleanList)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            foreach (string word in cleanList)
            {
                if (wordCount.ContainsKey(word))
                {
                    wordCount[word]++;
                }
                else
                {
                    wordCount.Add(word, 1);
                }
            }

            /*var uniqueWordCount = wordCount.GroupBy(x => x.Key)
                .Select(x => new { Word = x.Key, Count = x.Sum(y => y.Value) })
                .OrderBy(x => x.Count);*/


            var sortedWordCount = wordCount.OrderBy(x => x.Value);
            foreach (var item in sortedWordCount)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");

            }

            var top = wordCount.OrderByDescending(x => x.Value).Take(10);
            foreach (var item in top)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

    }
}