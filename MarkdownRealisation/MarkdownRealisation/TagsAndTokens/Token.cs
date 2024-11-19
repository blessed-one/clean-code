namespace MarkdownRealisation.TagsAndTokens
{
    public abstract class Token
    {
        public abstract bool IsTag { get; }
        public abstract Token Copy();
        public abstract override string ToString();
    }
}