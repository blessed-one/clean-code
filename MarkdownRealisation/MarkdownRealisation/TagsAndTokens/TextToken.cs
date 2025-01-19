namespace MarkdownRealisation.TagsAndTokens
{
    public class TextToken(string content) : Token
    {
        private string Content { get; set; } = content;

        public override string ToString()
        {
            return Content;
        }
    }
}