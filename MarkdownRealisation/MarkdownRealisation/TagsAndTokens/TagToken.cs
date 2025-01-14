 using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens
{
    public class TagToken : Token
    {
        private readonly string _htmlTag;
        private readonly string _mdTag;
        public override bool IsTag => true;
        public readonly TagType Type;
        private bool IsHtml { get; set; }
        public bool IsOpen { get; set; }
        public bool IsDisabled { get; set; } = false;
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

        public override TagToken Copy()
        {
            var copy = new TagToken(_mdTag, _htmlTag, Type, Position)
            {
                IsHtml = IsHtml,
                IsOpen = IsOpen
            };
            return copy;
        }
    }
}