// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using LrcParser.Model;
using LrcParser.Utils;

namespace LrcParser.Parser.Lrc.Utils;

internal static class LrcTimedTextUtils
{
    // technically should be @"\<(\d{2,}):(\d{2})\.(\d{2})\>", but might be small case that start time format is invalid.
    private static readonly Regex start_time_regex = new(@"\<(\d{1,}):(\d{1,})\.(\d{1,})\>");

    /// <summary>
    ///
    /// </summary>
    /// <param name="timedText"></param>
    /// <returns></returns>
    internal static Tuple<string, SortedDictionary<TextIndex, int>> TimedTextToObject(string timedText)
    {
        if (string.IsNullOrEmpty(timedText))
            return new Tuple<string, SortedDictionary<TextIndex, int>>("", new SortedDictionary<TextIndex, int>());

        var matchTimeTags = start_time_regex.Matches(timedText);

        var endTextIndex = timedText.Length;

        var startIndex = 0;

        var text = string.Empty;
        var timeTags = new SortedDictionary<TextIndex, int>();

        foreach (var match in matchTimeTags.ToArray())
        {
            var endIndex = match.Index;

            if (startIndex < endIndex)
            {
                // add the text.
                text += timedText[startIndex..endIndex];
            }

            // update the new start for next time-tag calculation.
            startIndex = endIndex + match.Length;

            // add the time-tag.
            var hasText = startIndex < endTextIndex;
            var isEmptyStringNext = hasText && timedText[startIndex] == ' ';

            var state = hasText && !isEmptyStringNext ? IndexState.Start : IndexState.End;
            var textIndex = text.Length - (state == IndexState.Start ? 0 : 1);
            var time = ConvertTimeTagToMillionSecond(match.Value);

            // using try add because it might be possible with duplicated time-tag position in the lyric.
            timeTags.TryAdd(new TextIndex(textIndex, state), time);
        }

        // should add remaining text at the right of the end time-tag.
        text += timedText[startIndex..endTextIndex];

        return new Tuple<string, SortedDictionary<TextIndex, int>>(text, timeTags);
    }

    /// <summary>
    /// Convert the &lt;1:00.00&gt; to 60000
    /// </summary>
    /// <param name="timeTag"></param>
    /// <returns></returns>
    internal static int ConvertTimeTagToMillionSecond(string timeTag)
    {
        Match match = start_time_regex.Match(timeTag);

        if (!match.Success)
            throw new InvalidOperationException("Time tag format is invalid.");

        int minutes = int.Parse(match.Groups[1].Value);
        int seconds = int.Parse(match.Groups[2].Value);
        int hundredths = int.Parse(match.Groups[3].Value);

        return minutes * 60 * 1000 + seconds * 1000 + hundredths * 10;
    }

    internal static string ToTimedText(string text, SortedDictionary<TextIndex, int> timeTags)
    {
        var insertIndex = 0;

        var timedText = text;

        foreach (var (textIndex, time) in timeTags)
        {
            var timeTagString = ConvertMillionSecondToTimeTag(time);
            var stringIndex = TextIndexUtils.ToGapIndex(textIndex);
            timedText = timedText.Insert(insertIndex + stringIndex, timeTagString);

            insertIndex += timeTagString.Length;
        }

        return timedText;
    }

    /// <summary>
    /// Convert the 60000 to &lt;1:00.00&gt;
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <returns></returns>
    internal static string ConvertMillionSecondToTimeTag(int milliseconds)
    {
        if (milliseconds < 0)
            throw new InvalidOperationException($"{nameof(milliseconds)} should be greater than 0.");

        int totalSeconds = milliseconds / 1000;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int hundredths = (milliseconds % 1000) / 10;

        return $"<{minutes:D2}:{seconds:D2}.{hundredths:D2}>";
    }
}
