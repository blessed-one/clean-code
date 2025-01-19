namespace MarkdownRealisation.MainClasses
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md();
            Console.WriteLine(md.RenderHtml("# ___asd___"));
        }
    }
}