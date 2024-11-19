using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;
using System.Text;

namespace MarkdownRealisation.MainClasses
{
    public class Md : IRender
    {
        private readonly Parser _parser = new Parser();
        private readonly Resolver _resolver = new Resolver();
        public string Render(string text)
        {
            var result = new StringBuilder();
            
            var tokens = _resolver.ResolveTokens(_parser.Parse(text));
            
            foreach ( var token in tokens )
            {
                if (token.IsTag)
                {
                    var tag = (TagToken)token;
                    if (!tag.IsOpen) tag.Convert();
                }
                result.Append(token.ToString());
            }

            return result.ToString();
        }
    }
}