using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class HeaderToken : TagToken
{
    public override string HtmlTag { get; }
    public override string MarkdownTag { get; }
    public override TagType Type => TagType.Header;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }
    private int HeaderLevel { get; init; }

    public HeaderToken(int headerLevel)
    {
        HeaderLevel = headerLevel;
        switch (headerLevel)
        {
            case 1:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;

            case 2:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;
            
            case 3:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;
            
            case 4:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;
            
            case 5:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;
            
            case 6:
                HtmlTag = "h1";
                MarkdownTag = "##";
                break;
            
            default:
                throw new Exception("Invalid header level");
        }
    }
    
    public override object Clone() => new HeaderToken(HeaderLevel)
    {
        Pair = (TagToken?)Pair?.Clone(),
    };
}