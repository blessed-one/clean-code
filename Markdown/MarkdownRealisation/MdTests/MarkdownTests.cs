using MarkdownRealisation.MainClasses;

namespace MdTests;

public class MarkdownTests
{
    [Fact]
    public void Italics_EmphasizeTextBetweenSingleUnderscores()
    {
        var md = new Md();
        string input = "Текст, _окруженный с двух сторон_ одинарными символами подчерка.";
        string expected = "<p>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void Bold_EmphasizeTextBetweenDoubleUnderscores()
    {
        var md = new Md();
        string input = "__Выделенный двумя символами текст__";
        string expected = "<p><strong>Выделенный двумя символами текст</strong></p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }
    
    [Fact]
    public void NestedTags_SupportsItalicInsideBold()
    {
        var md = new Md();
        string input = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
        string expected = "<p>Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void NestedTags_NotBoldInsideItalic()
    {
        var md = new Md();
        string input = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
        string expected = "<p>Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void Numbers_DoNotTriggerItalicFormatting()
    {
        var md = new Md();
        string input = "Подчерки внутри текста c цифрами_12_3 не считаются выделением.";
        string expected = "<p>Подчерки внутри текста c цифрами_12_3 не считаются выделением.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void DifferentWords_AreNotItalicizedTogether()
    {
        var md = new Md();
        string input = "В то же время выделение в ра_зных сл_овах не работает.";
        string expected = "<p>В то же время выделение в ра_зных сл_овах не работает.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void UnpairedUnderscores_DoNotTriggerFormatting()
    {
        var md = new Md();
        string input = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
        string expected = "<p>__Непарные_ символы в рамках одного абзаца не считаются выделением.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void LeadingOrTrailingSpaces_PreventFormatting()
    {
        var md = new Md();
        string input = "Эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";
        string expected = "<p>Эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void CrossingTags_DisablesAllFormatting()
    {
        var md = new Md();
        string input = "В случае __пересечения _двойных__ и одинарных_ подчерков ничего не выделяется.";
        string expected = "<p>В случае __пересечения _двойных__ и одинарных_ подчерков ничего не выделяется.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void EmptyStringInsideTags_DisablesFormatting()
    {
        var md = new Md();
        string input = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";
        string expected = "<p>Если внутри подчерков пустая строка ____, то они остаются символами подчерка.</p>";
        Assert.Equal(expected, md.RenderHtml(input));
    }

    [Fact]
    public void Headings_ApplyHeadingTag()
    {
        var md = new Md();
        string input = "# Заголовок __с _разными_ символами__";
        string expected = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";
        Assert.Equal(expected, md.RenderHtml(input));
    }
}