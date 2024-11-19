namespace MarkdownRealisation.MainClasses
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md();
            Console.WriteLine(md.Render("___aaa_ _ccc_ __ddd__ _bbb___"));
        }
    }
}