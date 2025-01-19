namespace MarkdownRealisation.MainClasses
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md();
            // Console.WriteLine(md.Render("asd"));
            
            var aa = new AA();

            Console.WriteLine(aa is A);
        }

        public abstract class A;

        public class AA : A;

    }
}