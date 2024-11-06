namespace MarkdownRealisation.TagsAndTokens
{
    public class TextToken : Token
    {
        public string Content { get; set; }

        public TextToken(string content)
        {
            Content = content;
        }

        public override string ToString()
        {
            return Content;
        }

        public override Token Copy()
        {
            return new TextToken(Content);
        }
    }
}