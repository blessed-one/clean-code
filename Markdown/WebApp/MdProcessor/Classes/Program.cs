namespace MdProcessor.Classes;

public static class Program
{
    public static void Main(string[] args)
    {
        var md = new Md();
        Console.WriteLine(md.RenderHtml("__aaa\n__aa__ "));
    }
}