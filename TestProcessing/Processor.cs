namespace TestProcessing;

public class Processor
{
    public const float WORD_READING_RATE_PER_MINUTE = 200.0F;
    private const string CODE_SNIPPET_MARKER = "```";

    public string Analyse(string input)
    {
        IEnumerable<string> words = GetWordsFrom(input);
        IEnumerable<string> topWords = GetTop10Words(words);
        string result = GetOutput(topWords, words.Count());

        return result;
    }

    public IEnumerable<string> GetWordsFrom(string input)
    {
        List<string> words = new();
        bool ignore = false;
        string[] strings = GetLinesFrom(input);
        string[] lines = strings;

        foreach (string line in lines) {
            if (line.StartsWith(CODE_SNIPPET_MARKER)) {
                ignore = !ignore;
                continue;
            }
            if (ignore)
                continue;

            words.AddRange(WordsFromLine(line));
        }

        return words;
    }

    private static string[] GetLinesFrom(string input)
        => input.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    private IEnumerable<string> WordsFromLine(string line)
    {
        List<string> words = new();
        string word = string.Empty;

        for (int i = 0; i < line.Length; i++) {
            if (char.IsLetter(line[i]))
                word += line[i];
            if (WordHasEnded(line, i) && !string.IsNullOrEmpty(word)) {
                words.Add(word.ToLower());
                word = string.Empty;
            }
        }

        return words;
    }

    private static bool WordHasEnded(string line, int index)
        => (!char.IsLetter(line[index]) || LastCharacterInLine(line, index));

    private static bool LastCharacterInLine(string line, int index)
        => index == line.Length - 1;

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

    public int GetReadingTimeInMinutes(string input)
    {
        float rawReadingTime = GetWordsFrom(input).Count() / WORD_READING_RATE_PER_MINUTE;

        return (int)Math.Round(rawReadingTime, 0, MidpointRounding.AwayFromZero);
    }
}
