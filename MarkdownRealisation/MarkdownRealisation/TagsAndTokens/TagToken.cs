 using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens
{
    public abstract class TagToken : Token, ICloneable
    {
        protected readonly string _htmlTag;
        protected readonly string _mdTag;

        public readonly TagType Type;
        private bool IsHtml { get; set; }
        public bool IsOpen { get; set; }
        public TagPosition Position { get; set; }
        public TagToken? Pair {  get; set; }

        public TagToken(string md, string html, TagType type)
        {
            _mdTag = md;
            _htmlTag = html;
            Type = type;
            IsHtml = false;
            IsOpen = true;
        }
        public TagToken(string md, string html, TagType type, TagPosition pos)
        {
            _mdTag = md;
            _htmlTag = html;
            Type = type;
            IsHtml = false;
            IsOpen = true;
            Position = pos;
        }

        protected TagToken()
        {
        }

        public void Convert()
        {
            IsHtml = !IsHtml;
        }
        public override string ToString()
        {
            if (!IsHtml) return _mdTag;

            return Position switch
            {
                TagPosition.Start => $"<{_htmlTag}>",
                TagPosition.End => $"</{_htmlTag}>",
                _ => _mdTag
            };
        }

        public abstract object Clone();
    }
}