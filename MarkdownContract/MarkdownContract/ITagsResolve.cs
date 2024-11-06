using MarkdownContract;

public interface ITagsResolve
{
    /// <summary>
    /// Обрабатывает теги, обозначает их как открытые/закрытые
    /// </summary>
    /// <param name="tokens">Список тегов на обработку</param>
    /// <returns>Обработанный список тегов</returns>
    public List<Token> ResolveTokens(List<Token> tokens);
}