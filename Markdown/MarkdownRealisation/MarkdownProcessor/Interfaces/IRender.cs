namespace MarkdownRealisation.Interfaces;

public interface IRender
{
    /// <summary>
    /// Конвертирует текст в html разметку
    /// </summary>
    /// <param name="text">Текст с исходной разметкой</param>
    /// <returns>Текст с html разметкой</returns>
    public string RenderHtml(string text);
}