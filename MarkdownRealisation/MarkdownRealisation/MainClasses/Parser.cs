using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.MainClasses
{
    public class Parser : IParse, ITagsResolve
    {
        public Token[] Parse(string text)
        {
            var result = new List<Token>();

            List<string> lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var line in lines)
            {
                var lineResult = new List<Token>();
                bool isHeaderChecked = false;
                foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!isHeaderChecked)
                    {
                        isHeaderChecked = true;
                        switch (word)
                        {
                            case "######":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            case "#####":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            case "####":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            case "###":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            case "##":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            case "#":
                                lineResult.Add(TagLibrary.GetTagToken(word));
                                break;
                            default:
                                lineResult.Add(new TagToken("", "p", TagType.Paragraph, TagPosition.Start));
                                break;
                        }
                    }

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
                            tempWord = tempWord.Substring(tempWord.Length - tag.Length);
                            tempTokenStack.Push(newTag);
                        }
                    }

                    lineResult.Add(new TextToken(tempWord));

                    foreach (var tagToken in tempTokenStack)
                    {
                        lineResult.Add(tagToken);
                    }

                    var lastlineTagToken = (TagToken)lineResult.First().Copy();
                    lastlineTagToken.Position = TagPosition.End;
                    lineResult.Add(lastlineTagToken);
                }
            }

            return result.ToArray();
        }

        public Token[] ResolveTokens(Token[] tokens)
        {
            var tokenStack = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.isTag)
                {
                    if (!tokenStack.Any())
                    {
                        tokenStack.Push(token);
                    }
                    else
                    {
                        TagToken current = (TagToken)token;
                        TagToken previous = (TagToken)tokenStack.Peek();

                        if (previous.type == TagType.Header) tokenStack.Push(current);
                        else if (previous.type == current.type)
                        {
                            /// ()
                            if (previous.Position == TagPosition.Start && current.Position == TagPosition.End)
                            {
                                previous.IsOpen = false;
                                current.IsOpen = false;
                                tokenStack.Pop();
                            }
                            /// )(
                            else if (previous.Position == TagPosition.End && current.Position == TagPosition.Start)
                            {
                                tokenStack.Pop();
                                tokenStack.Push(token);
                            }
                            /// ))
                            else if (previous.Position == TagPosition.End && current.Position == TagPosition.End)
                            {
                                tokenStack.Pop();
                            }
                            /// ((
                            else if (previous.Position == TagPosition.Start && current.Position == TagPosition.Start)
                            {
                                tokenStack.Push(token);
                            }
                        }
                        else
                        {
                            /// {)
                            if (previous.Position == TagPosition.Start && current.Position == TagPosition.End)
                            {
                                continue;
                            }
                            /// }(
                            else if (previous.Position == TagPosition.End && current.Position == TagPosition.Start)
                            {
                                tokenStack.Pop();
                                tokenStack.Push(token);
                            }
                            /// })
                            else if (previous.Position == TagPosition.End && current.Position == TagPosition.End)
                            {
                                tokenStack.Pop();
                            }
                            /// {(
                            else if (previous.Position == TagPosition.Start && current.Position == TagPosition.Start)
                            {
                                tokenStack.Push(token);
                            }
                        }
                    }
                }
            }

            return tokenStack.ToArray();
        }
    }
}