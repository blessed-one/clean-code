namespace MarkdownRealisation.MainClasses
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var a = " \t";
            foreach (var smb in a)
            {
                Console.WriteLine(smb.GetHashCode());
            }
        }
    }
}