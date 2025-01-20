namespace MarkdownContract
{
    public interface IRender
    {
        /// <summary>
        /// Конвертирует текст из одной разметки в другую
        /// </summary>
        /// <param name="text">Текст с исходной разметкой</param>
        /// <returns>Текст с изменённой разметкой</returns>
        public string Render(string text);
    }
}
