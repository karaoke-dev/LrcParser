// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Parser.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Parser.Lrc.Utils;

namespace LrcParser.Parser.Lrc.Lines;

public class LrcLyricParser : SingleLineParser<LrcLyric>
{
    public override bool CanDecode(string text)
        => !string.IsNullOrWhiteSpace(text);

    public override LrcLyric Decode(string text)
    {
        var (lyric, timeTags) = LrcTimedTextUtils.TimedTextToObject(text);
        return new LrcLyric
        {
            Text = lyric,
            TimeTags = timeTags
        };
    }

    public override string Encode(LrcLyric component, int index)
    {
        return LrcTimedTextUtils.ToTimedText(component.Text, component.TimeTags);
    }
}
