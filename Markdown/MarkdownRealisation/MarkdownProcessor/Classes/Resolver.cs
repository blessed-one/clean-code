using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;
using MarkdownRealisation.Interfaces;
using MarkdownRealisation.TagsAndTokens;

namespace MarkdownRealisation.Classes;

public class Resolver : ITagsResolver
{
    private List<TagToken> _closedTagTokens = new List<TagToken>();


    private void CloseFirstToken(Token[] tokens)
    {
        var firstToken = tokens.First() as TagToken;
        _closedTagTokens.Add(firstToken);
        _closedTagTokens.Add(firstToken.Pair);
    }

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
            .ToArray();

        foreach (var token in tokensToResolve)
        {
            if (_closedTagTokens.Contains(token)) continue;

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
                _closedTagTokens.Add(previous);
                previous.Pair = current;

                _closedTagTokens.Add(current);
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

            if (_closedTagTokens.Contains(tokenA) && tokenA.Position == TagPosition.Start)
            {
                for (var j = i + 1; j < tokensToResolve.Count; j++)
                {
                    var tokenB = tokensToResolve[j];
                    if (tokenA.Pair == tokenB) break;

                    if (!_closedTagTokens.Contains(tokenB)) continue;
                    if (tokenA.Type == TagType.Bold && tokenB.Type == TagType.Italic) continue;

                    _closedTagTokens.Remove(tokenB);
                    _closedTagTokens.Remove(tokenB.Pair!);
                }
            }
        }

        return tokens;
    }

    private Token[] OpenedTagsToText(Token[] tokens)
    {
        var result = new List<Token>();

        foreach (var token in tokens)
        {
            if (token is TagToken && !_closedTagTokens.Contains(token))
            {
                result.Add(new TextToken(((TagToken)token).MarkdownTag));
            }
            else
            {
                result.Add(token);
            }
        }

        return result.ToArray();
    }

    public Token[] ResolveTokens(Token[] tokens)
    {
        return OpenedTagsToText(ResolveTagsInteractions(ResolvePairs(tokens)));
    }

    public Token[] ResolveTokensLines(Token[][] tokensLines)
    {
        return tokensLines
            .Select(x =>
            {
                CloseFirstToken(x);
                return ResolveTokens(x);
            })
            .SelectMany(x => x).ToArray();
    }
}