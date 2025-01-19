using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;
using System.Text;

namespace MarkdownRealisation.MainClasses
{
    public class Md : IRender
    {
        private readonly Parser _parser = new();
        private readonly Resolver _resolver = new();
        public string Render(string text)
        {
            var result = new StringBuilder();
            
            var tokens = _resolver.ResolveTokens(_parser.Parse(text));
            
            foreach ( var token in tokens )
            {
                if (token is TagToken)
                {
                    var tag = (TagToken)token;
                    if (!tag.IsOpen) tag.Convert();
                }
                result.Append(token);
            }

            return result.ToString();
        }
    }
}