using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class ItalicToken : TagToken
{
    public string HtmlTag => "em";
    public string MarkdownTag => "_";
    public TagType Type => TagType.Italic;
    public TagPosition Position { get; set; }
    
    public override object Clone()
    {
        return new ItalicToken();
    }
}