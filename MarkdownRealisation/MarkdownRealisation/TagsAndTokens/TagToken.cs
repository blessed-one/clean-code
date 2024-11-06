using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens
{
    public class TagToken : Token
    {
        public readonly string htmlTag;
        public readonly string mdTag;
        public TagType type;
        public bool IsHTML { get; private set; }
        public bool IsOpen { get; set; }
        public TagPosition Position { get; set; }

        public TagToken(string md, string html, TagType type)
        {
            mdTag = md;
            htmlTag = html;
            this.type = type;
            IsHTML = false;
            IsOpen = true;
        }
        public TagToken(string md, string html, TagType type, TagPosition pos)
        {
            mdTag = md;
            htmlTag = html;
            this.type = type;
            IsHTML = false;
            IsOpen = true;
            Position = pos;
        }

        public TagToken()
        {
        }

        public void Convert()
        {
            IsHTML = !IsHTML;
        }
        public override string ToString()
        {
            return IsHTML ? htmlTag : mdTag;
        }

        public override TagToken Copy()
        {
            var copy = new TagToken(mdTag, htmlTag, type, Position)
            {
                IsHTML = IsHTML,
                IsOpen = IsOpen
            };
            return copy;
        }
    }
}