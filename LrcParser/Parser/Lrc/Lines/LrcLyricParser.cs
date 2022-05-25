// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using LrcParser.Model;
using LrcParser.Parser.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Parser.Lrc.Utils;
using LrcParser.Utils;

namespace LrcParser.Parser.Lrc.Lines;

public class LrcLyricParser : SingleLineParser<LrcLyric>
{
    public override bool CanDecode(string text)
        => !string.IsNullOrWhiteSpace(text);

    public override LrcLyric Decode(string text)
    {
        if (!CanDecode(text))
            return new LrcLyric();

        var timeTagRegex = new Regex(@"\[\d\d:\d\d[:.]\d\d\]");
        var matchTimeTags = timeTagRegex.Matches(text);

        var endTextIndex = text.Length;

        var lyric = new LrcLyric();
        var startIndex = 0;

        foreach (var match in matchTimeTags.ToArray())
        {
            var endIndex = match.Index;
            if (startIndex < endIndex)
            {
                // add the lyric.
                lyric.Text += text[startIndex..endIndex];
            }

            // update the new start for next time-tag calculation.
            startIndex = endIndex + match.Length;

            // add the time-tag.
            var hasText = startIndex < endTextIndex;
            var textIndex = lyric.Text.Length - (hasText ? 0 : 1);
            var state = hasText ? IndexState.Start : IndexState.End;
            var time = TimeTagUtils.TimeTagToMillionSecond(match.Value);
            lyric.TimeTags.Add(new TextIndex(textIndex, state), time);
        }

        // should add remaining text at the right of the end time-tag.
        lyric.Text += text[startIndex..endTextIndex];

        return lyric;
    }

    public override string Encode(LrcLyric component, int index)
    {
        var text = component.Text;

        var insertIndex = 0;

        foreach (var (textIndex, time) in component.TimeTags)
        {
            if(time == null)
                continue;

            var timeTagString = TimeTagUtils.MillionSecondToTimeTag(time.Value);
            var stringIndex = TextIndexUtils.ToStringIndex(textIndex);
            text = text.Insert(insertIndex + stringIndex, timeTagString);

            insertIndex += timeTagString.Length;
        }

        return text;
    }
}
