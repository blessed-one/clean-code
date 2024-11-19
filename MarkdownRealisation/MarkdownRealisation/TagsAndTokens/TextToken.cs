namespace MarkdownRealisation.TagsAndTokens
{
    public class TextToken(string content) : Token
    {
        private string Content { get; set; } = content;
        public override bool IsTag => false;

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