using TestProcessing;

namespace TextProcessingTests;

public class ProcessorTests
{
    private Processor processor;

    [SetUp]
    public void SetUp()
        => processor = new();

    [Test]
    public void GetWords()
    {
        string input = "Hello, this is an example. Your case.";
        string[] expectedWords = new string[] { "hello", "this", "is", "an", "example", "your", "case" };

        IEnumerable<string> result = processor.GetWordsFrom(input);

        Assert.That(result, Is.EquivalentTo(expectedWords));
    }

    [Test]
    public void Get10TopWords()
    {
        string input = "Hello, this is an example for you to practice. You should grab this text and make it as your test case.";
        string[] expectedWords = new string[] { "this", "you", "hello", "is", "an", "example", "for", "to", "practice", "should" };

        IEnumerable<string> result = processor.GetTop10Words(processor.GetWordsFrom(input));

        Assert.That(result, Is.EquivalentTo(expectedWords));
    }

    [Test]
    public void GetOutput()
    {
        string input = "Hello, this is an example for you to practice. You should grab this text and make it as your test case.";

        string result = processor.Analyse(input);

        Assert.That(result,
            Is.EqualTo(
                "Those are the top 10 words used:\r\n\r\n" +
                "1. this\r\n" +
                "2. you\r\n" +
                "3. hello\r\n" +
                "4. is\r\n" +
                "5. an\r\n" +
                "6. example\r\n" +
                "7. for\r\n" +
                "8. to\r\n" +
                "9. practice\r\n" +
                "10. should\r\n\r\n" +
                "The text has in total 21 words"
            )
        );
    }

    [Test]
    public void GetOutputWithEmptyInput()
    {
        string input = string.Empty;
        string result = processor.Analyse(input);
        Assert.That(result,
        Is.EqualTo(
                "Those are the top 10 words used:\r\n\r\n\r\n" +
                "The text has in total 0 words"
            )
        );
    }

    [Test]
    public void GetReadingTime_ExactlyOneMinute()
    {
        string input = string.Concat(Enumerable.Repeat("word ", (int)Processor.WORD_READING_RATE_PER_MINUTE));

        int result = processor.GetReadingTimeInMinutes(input);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GetReadingTime_ZeroWords()
    {
        string input = string.Empty;

        int result = processor.GetReadingTimeInMinutes(input);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetReadingTime_RoundingDown()
    {
        string input = string.Concat(Enumerable.Repeat("word ", 622));

        int result = processor.GetReadingTimeInMinutes(input);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void GetReadingTime_RoundingUp()
    {
        string input = string.Concat(Enumerable.Repeat("word ", 738));

        int result = processor.GetReadingTimeInMinutes(input);

        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void CodeSnippetsShouldBeIgnored()
    {
        string input = "Hello, this is an example for you to practice. You should grab\r\n" +
            "this text and make it as your test case:\r\n\r\n" +
            "```javascript\r\n" +
            "if (true) {" +
            "\r\n" +
            "  console.log('should should should')\r\n" +
            "}\r\n" +
            "```\r\n\r\n";

        string result = processor.Analyse(input);

        Assert.That(result,
            Is.EqualTo(
                "Those are the top 10 words used:\r\n\r\n" +
                "1. this\r\n" +
                "2. you\r\n" +
                "3. hello\r\n" +
                "4. is\r\n" +
                "5. an\r\n" +
                "6. example\r\n" +
                "7. for\r\n" +
                "8. to\r\n" +
                "9. practice\r\n" +
                "10. should\r\n\r\n" +
                "The text has in total 21 words"
            )
        );
    }
}