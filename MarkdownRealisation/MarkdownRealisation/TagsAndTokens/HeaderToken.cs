using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class HeaderToken : TagToken
{
    public string HtmlTag { get; private set; }
    public string MarkdownTag { get; private set; }
    public TagType Type => TagType.Italic;
    public TagPosition Position { get; set; }
    public int HeaderLevel { get; init; }

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
    
    public override object Clone()
    {
        return new HeaderToken(HeaderLevel);
    }
}