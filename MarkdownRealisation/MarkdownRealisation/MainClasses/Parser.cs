using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.MainClasses
{
    public class Parser : IParse
    {
        private (string[] RemainingWords, TagToken OpeningTag) FindOpeningTag(string[] words)
        {

            (string[] RemainingWords, TagToken OpeningTag) result;
            
            var firstWord = words.First();
            switch (firstWord)
            {
                case "######":
                case "#####":
                case "####":
                case "###":
                case "##":
                case "#":
                    var headerTag = TagLibrary.GetTagToken(firstWord);
                    result.OpeningTag = headerTag;
                    result.RemainingWords = words.Skip(1).ToArray();
                    break;
                default:
                    var paragraphTag = new ParagraphToken();
                    result.OpeningTag = paragraphTag;
                    result.RemainingWords = words;
                    break;
            }
            
            return result;
        }

        private Token[] ParseOneWord(string word)
        {
            var result = new List<Token>();
            
            if (word.All(c => c.Equals('_')))
            {
                result.Add(new TextToken(word));
                return result.ToArray();
            }
                    
            string tempWord = word;
            foreach (var tag in TagLibrary.Tags)
            {
                if (tempWord.StartsWith(tag))
                {
                    var newTag = TagLibrary.GetTagToken(tag);
                    tempWord = tempWord.Substring(tag.Length);

                    result.Add(newTag);
                }
            }

            var tempTokenStack = new Stack<Token>();
            foreach (var tag in TagLibrary.Tags)
            {
                if (tempWord.EndsWith(tag))
                {
                    var newTag = TagLibrary.GetTagToken(tag);
                    newTag.Position = TagPosition.End;
                    tempWord = tempWord.Substring(0, tempWord.Length - tag.Length);
                    tempTokenStack.Push(newTag);
                }
            }

            result.Add(new TextToken(tempWord));

            while (tempTokenStack.Count != 0)
            {
                result.Add(tempTokenStack.Pop());
            }

            return result.ToArray();
        }
        
        public Token[] Parse(string text)
        {
            var result = new List<Token>();

            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var lineResult = new List<Token>();
                var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var opening = FindOpeningTag(words);
                words = opening.RemainingWords;
                lineResult.Add(opening.OpeningTag);
                
                foreach (var word in words)
                {
                    if (word != words.First()) 
                        lineResult.Add(new TextToken(" "));

                    lineResult.AddRange(ParseOneWord(word));
                }
                
                // Creating a pair for the first token
                var firstLineTagToken = (TagToken)lineResult.First();
                var lastLineTagToken = (TagToken)firstLineTagToken.Clone();
                lastLineTagToken.Position = TagPosition.End;
                lastLineTagToken.Pair = firstLineTagToken;
                firstLineTagToken.Pair = lastLineTagToken;
                lineResult.Add(lastLineTagToken);
                
                result.AddRange(lineResult);
            }

            return result.ToArray();
        }
    }
}