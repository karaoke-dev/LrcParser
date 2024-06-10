// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Parser.Lines;
using LrcParser.Parser.Kar.Metadata;
using LrcParser.Parser.Kar.Utils;

namespace LrcParser.Parser.Kar.Lines;

public class KarLyricParser : SingleLineParser<KarLyric>
{
    public override bool CanDecode(string text)
        => !string.IsNullOrWhiteSpace(text);

    public override KarLyric Decode(string text)
    {
        var (lyric, timeTags) = KarTimedTextUtils.TimedTextToObject(text);
        return new KarLyric
        {
            Text = lyric,
            TimeTags = timeTags,
        };
    }

    public override string Encode(KarLyric component, int index)
    {
        return KarTimedTextUtils.ToTimedText(component.Text, component.TimeTags);
    }
}
