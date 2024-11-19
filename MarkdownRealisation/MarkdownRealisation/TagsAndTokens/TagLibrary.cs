using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens
{
    public static class TagLibrary
    {
        public static readonly string[] Tags = ["__", "_"];

        public static TagToken GetTagToken(string mdTag)
        {
            return mdTag switch
            {
                "######" => new TagToken("######", "h6", TagType.Header, TagPosition.Start),
                "#####"  => new TagToken("#####", "h5", TagType.Header, TagPosition.Start),
                "####"   => new TagToken("####", "h4", TagType.Header, TagPosition.Start),
                "###"    => new TagToken("###", "h3", TagType.Header, TagPosition.Start),
                "##"     => new TagToken("##", "h2", TagType.Header, TagPosition.Start),
                "#"      => new TagToken("#", "h1", TagType.Header, TagPosition.Start),
                "__"     => new TagToken("__", "strong", TagType.Bold, TagPosition.Start),
                "_"      => new TagToken("_", "em", TagType.Italic, TagPosition.Start),
                _        => throw new Exception($"Unknown tag: {mdTag}")
            };
        }
    }
}