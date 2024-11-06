using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens
{
    public static class TagLibrary
    {
        private readonly static Dictionary<string, TagToken> TagDictonary = new()
    {
        { "######", new TagToken("######", "h6",      TagType.Header6, TagPosition.Start)},
        { "#####" , new TagToken("#####" , "h5",      TagType.Header5, TagPosition.Start)},
        { "####"  , new TagToken("####"  , "h4",      TagType.Header4, TagPosition.Start)},
        { "###"   , new TagToken("###"   , "h3",      TagType.Header3, TagPosition.Start)},
        { "##"    , new TagToken("##"    , "h2",      TagType.Header2, TagPosition.Start)},
        { "#"     , new TagToken("#"     , "h1",      TagType.Header1, TagPosition.Start)},
        { "__"    , new TagToken("__"    , "strong",  TagType.Bold,    TagPosition.Start)},
        { "_"     , new TagToken("_"     , "em",      TagType.Italic,  TagPosition.Start)}
    };

        public readonly static string[] Tags = { "__", "_" };

        public static TagToken GetTagToken(string mdTag)
        {
            return TagDictonary[mdTag];
        }
    }
}