namespace MarkdownRealisation.Classes;

public static class Program
{
    public static void Main(string[] args)
    {
        var md = new Md();
        Console.WriteLine(md.RenderHtml(""));
    }
}