using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.MainClasses
{
    public class Parser : IParse
    {
        public Token[] Parse(string text)
        {
            var result = new List<Token>();

            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var lineResult = new List<Token>();
                var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
                        headerTag.IsOpen = false;
                        lineResult.Add(headerTag);
                        words = words.Skip(1).ToArray();
                        break;
                    default:
                        var paragraphTag = new TagToken("", "p", TagType.Paragraph, TagPosition.Start)
                        {
                            IsOpen = false
                        };
                        lineResult.Add(paragraphTag);
                        break;
                }
                
                foreach (var word in words)
                {
                    if (word != words.First()) lineResult.Add(new TextToken(" "));
                    string tempWord = word;
                    foreach (var tag in TagLibrary.Tags)
                    {
                        if (tempWord.StartsWith(tag))
                        {
                            var newTag = TagLibrary.GetTagToken(tag);
                            tempWord = tempWord.Substring(tag.Length);

                            lineResult.Add(newTag);
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

                    lineResult.Add(new TextToken(tempWord));

                    while (tempTokenStack.Count != 0)
                    {
                        lineResult.Add(tempTokenStack.Pop());
                    }
                }
                var firstLineTagToken = (TagToken)lineResult.First();
                var lastLineTagToken = firstLineTagToken.Copy();
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