namespace MarkdownRealisation;

public static class EnumerableExtensions
{
    public static void Print<T>(this IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            Console.WriteLine(item);
        }
    }
}