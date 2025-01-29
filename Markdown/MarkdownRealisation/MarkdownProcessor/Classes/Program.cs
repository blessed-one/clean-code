namespace MarkdownRealisation.Classes;

public static class Program
{
    public static void Main(string[] args)
    {
        var md = new Md();
        try
        {
            Console.WriteLine( md.RenderHtml("1. asd\n    2. asd\n    3. asd\n4. asd"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}