using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.MainClasses;

public class Resolver : ITagsResolve
{
    /// <summary>
    /// Обозначает теги закрытыми если у них есть пара без пересечений
    /// </summary>
    /// <param name="tokens">Массив токенов на обработку</param>
    /// <returns>Массив обработанных токенов</returns>
    private Token[] ResolvePairs(Token[] tokens)
    {
        var tokenStack = new Stack<TagToken>();
        var tokensToResolve = tokens
            .OfType<TagToken>()
            .Where(token => token.IsOpen)
            .ToArray();

        foreach (var token in tokensToResolve)
        {
            if (!tokenStack.Any())
            {
                tokenStack.Push(token);
                continue;
            }

            var previous = tokenStack.Peek();
            var current = token;

            if (previous.Type == current.Type &&
                previous.Position == TagPosition.Start && current.Position == TagPosition.End)
            {
                previous.IsOpen = false;
                previous.Pair = current;

                current.IsOpen = false;
                current.Pair = previous;

                tokenStack.Pop();
            }
            else
            {
                tokenStack.Push(token);
            }
        }

        return tokens;
    }

    /// <summary>
    /// Обозначает теги открытыми если они не удовлетворяют пункту "Взаимодействие тегов" из спецификации
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    private Token[] ResolveTagsInteractions(Token[] tokens)
    {
        var tokensToResolve = tokens
            .OfType<TagToken>()
            .ToList();

        for (var i = 0; i < tokensToResolve.Count; i++)
        {
            var tokenA = tokensToResolve[i];
            if (tokenA.Type == TagType.Header || tokenA.Type == TagType.Paragraph) continue;

            if (tokenA is { IsOpen: false, Position: TagPosition.Start })
            {
                for (var j = i + 1; j < tokensToResolve.Count; j++)
                {
                    var tokenB = tokensToResolve[j];
                    if (tokenA.Pair == tokenB) break;

                    if (tokenB.IsOpen) continue;
                    if (tokenA.Type == TagType.Bold && tokenB.Type == TagType.Italic) continue;

                    tokenB.IsOpen = true;
                    tokenB.Pair!.IsOpen = true;
                }
            }
        }

        return tokens;
    }

    public Token[] ResolveTokens(Token[] tokens)
    {
        return ResolveTagsInteractions(ResolvePairs(tokens));
    }
}