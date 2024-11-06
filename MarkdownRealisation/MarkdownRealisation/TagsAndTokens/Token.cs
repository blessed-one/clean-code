namespace MarkdownRealisation.TagsAndTokens
{
    public abstract class Token
    {
        public bool isTag = false;
        public abstract Token Copy();
    }
}