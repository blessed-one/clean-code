using System.Text;
using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;

namespace MarkdownRealisation.Classes;

public class Md : IRender
{
    private readonly Parser _parser = new();
    private readonly Resolver _resolver = new();
    public string RenderHtml(string text)
    {
        var result = new StringBuilder();
            
        var tokens = _resolver.ResolveTokens(_parser.Parse(text));
            
        foreach ( var token in tokens )
        {
            if (token is TagToken)
            {
                var tag = (TagToken)token;
                switch (tag.Position)
                {
                    case TagPosition.Start:
                        result.Append($"<{tag.HtmlTag}>");
                        continue;
                    case TagPosition.End:
                        result.Append($"</{tag.HtmlTag}>");
                        continue;
                    default:
                        throw new Exception($"Unknown tag position: {tag.Position}");
                }
            }
                
            result.Append(token);
        }

        return result.ToString();
    }
}