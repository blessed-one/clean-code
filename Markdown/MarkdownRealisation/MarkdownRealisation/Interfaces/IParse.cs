using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.Interfaces
{
    public interface IParse
    {
        /// <summary>
        /// Приводит исходный массив строк в массив токенов
        /// </summary>
        /// <param name="text">Массив строк .md файла</param>
        /// <returns>Массив токенов</returns>
        public Token[] Parse(string text);
    }
}