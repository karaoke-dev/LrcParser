// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Parser.Lines;

namespace LrcParser.Parser;

/// <summary>
/// Base abstract class for encode/decode lyric format.
/// </summary>
public abstract class LyricParser
{
    private readonly IList<ISingleLineParser> parsers = new List<ISingleLineParser>();

    protected void Register<TParser>() where TParser : ISingleLineParser, new()
    {
        parsers.Add(new TParser());
    }

    /// <summary>
    /// Decode the lyric from the text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Song Decode(string text)
    {
        if (string.IsNullOrEmpty(text))
            return new Song();

        var lines = text.Split(
            ["\r\n", "\r", "\n"],
            StringSplitOptions.None
        ).Where(x => !string.IsNullOrWhiteSpace(x));

        // Generate all the objects.
        var objects = lines.Select(x =>
        {
            var parser = parsers.FirstOrDefault(p => p.CanDecode(x));

            return parser?.Decode(x);
        }).Where(x => x != null).Select(x => x!).ToList();

        // than pass to the post process.
        return PostProcess(objects);
    }

    protected abstract Song PostProcess(List<object> values);

    /// <summary>
    /// Encode the lyric to the text format.
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public string Encode(Song song)
    {
        // read all property.
        var objects = PreProcess(song);

        var index = 0;
        Type lastType = typeof(object);

        // convert to lines.
        var lines = objects.Select(x =>
        {
            if (lastType != x.GetType())
            {
                index = 0;
                lastType = x.GetType();
            }
            else
            {
                index++;
            }

            var parser = parsers.FirstOrDefault(p => p.CanEncode(x));

            return parser?.Encode(x, index) ?? "";
        }).ToList();

        return string.Join('\n', lines);
    }

    protected abstract IEnumerable<object> PreProcess(Song song);
}
