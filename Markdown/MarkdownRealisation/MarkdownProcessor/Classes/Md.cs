using System.Text;
using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.Classes;

public class Md : IRender
{
    private readonly Parser _parser = new();
    private readonly Resolver _resolver = new();
    public string RenderHtml(string text)
    {
        var result = new StringBuilder();
            
        var tokens = _resolver.ResolveTokensLines(_parser.Parse(text));
            
        foreach ( var token in tokens )
        {
            if (token is not TagToken)
            {
                result.Append(token);
                continue;
            }

            var tag = (TagToken)token;
            switch (tag.Position)
            {
                case TagPosition.Start:
                    result.Append($"<{tag.HtmlTag}>");
                    break;
                case TagPosition.End:
                    result.Append($"</{tag.HtmlTag}>");
                    //if (tag is HeaderToken or ParagraphToken) result.Append('\n');
                    break;
                default:
                    throw new Exception($"Unknown tag position: {tag.Position}");
            }
        }

        return result.ToString();
    }
}