namespace MarkdownRealisation.Classes;

public static class Program
{
    public static void Main(string[] args)
    {
        var md = new Md();
        try
        {
            Console.WriteLine( md.RenderHtml(
                "1. Первый уровень\n    1. Второй уровень\n        1. Третий уровень\n    2. Второй элемент второго уровня"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}