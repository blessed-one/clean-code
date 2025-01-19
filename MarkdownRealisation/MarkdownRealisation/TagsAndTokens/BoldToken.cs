using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class BoldToken : TagToken
{
    public string HtmlTag => "strong";
    public string MarkdownTag => "__";
    public TagType Type => TagType.Bold;
    public TagPosition Position { get; set; }

    public override object Clone()
    {
        return new BoldToken();
    }
}