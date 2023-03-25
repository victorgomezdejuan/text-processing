namespace TestProcessing;
public class Processor
{
    public string Analyse(string input)
    {
        IEnumerable<string> words = GetWords(input);
        IEnumerable<string> topWords = GetTop10Words(words);
        string result = GetOutput(topWords, words.Count());

        return result;
    }

    public IEnumerable<string> GetWords(string input)
    {
        List<string> words = new();
        string word = string.Empty;

        foreach (char character in input) {
            if (char.IsLetter(character))
                word += character;
            else if (!string.IsNullOrEmpty(word)) {
                words.Add(word.ToLower());
                word = string.Empty;
            }
        }

        return words;
    }

    public IEnumerable<string> GetTop10Words(IEnumerable<string> words)
    {
        Dictionary<string, int> wordCounts = new();

        foreach (string word in words) {
            if (wordCounts.ContainsKey(word))
                wordCounts[word]++;
            else
                wordCounts.Add(word, 1);
        }

        return wordCounts.OrderByDescending(wc => wc.Value).Take(10).Select(wd => wd.Key);
    }

    private static string GetOutput(IEnumerable<string> topWords, int wordCount)
    {
        string result = "Those are the top 10 words used:\r\n\r\n";
        int position = 1;
        foreach (string word in topWords) {
            result += $"{position}. {word}\r\n";
            position++;
        }
        result += $"\r\nThe text has in total {wordCount} words";
        return result;
    }
}
