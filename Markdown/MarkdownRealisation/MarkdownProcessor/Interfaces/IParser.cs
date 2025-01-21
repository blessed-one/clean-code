using MarkdownRealisation.Abstractions;

namespace MarkdownRealisation.Interfaces;

public interface IParser
{
    /// <summary>
    /// Приводит исходный массив строк в массив токенов
    /// </summary>
    /// <param name="text">Массив строк .md файла</param>
    /// <returns>Массив токенов</returns>
    public Token[] Parse(string text);
}