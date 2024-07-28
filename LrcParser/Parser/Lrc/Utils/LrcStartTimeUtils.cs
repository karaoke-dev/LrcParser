// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;

namespace LrcParser.Parser.Lrc.Utils;

public class LrcStartTimeUtils
{
    // technically should be @"\[(\d{2,}):(\d{2})\.(\d{2})\]", but might be small case that start time format is invalid.
    private static readonly Regex start_time_regex = new(@"\[(\d{1,}):(\d{1,})\.(\d{1,})\]");

    /// <summary>
    /// Separate the lyric format from [100:00.00][100:02.00] When the truth is found to be lies.
    /// to:
    /// [100:00.00][100:02.00]
    /// When the truth is found to be lies
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    internal static Tuple<int[], string> SplitLyricAndTimeTag(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return new Tuple<int[], string>([], string.Empty);

        // get all matched startTime
        MatchCollection matches = start_time_regex.Matches(line);

        var startTimes = matches.Select(x => ConvertTimeTagToMillionSecond(x.Value)).ToArray();
        var lyric = start_time_regex.Replace(line, "").Trim();

        return new Tuple<int[], string>(startTimes, lyric);
    }

    /// <summary>
    /// Convert the [1:00.00] to 60000
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

    /// <summary>
    /// Combine the lyric format from:
    /// [60000, 66000]
    /// When the truth is found to be lies
    /// to:
    /// [01:00.00][01:06.00] When the truth is found to be lies
    /// </summary>
    /// <param name="startTimes"></param>
    /// <param name="lyric"></param>
    /// <returns></returns>
    internal static string JoinLyricAndTimeTag(int[] startTimes, string lyric)
    {
        if (startTimes.Any() == false)
            throw new InvalidOperationException("Should contains at least one start time.");

        if (start_time_regex.Matches(lyric).Any())
            throw new InvalidOperationException("lyric should not contains any start time-tag info.");

        if (startTimes.Length == 0)
            return lyric;

        var result = startTimes.Aggregate(string.Empty, (current, t) => current + ConvertMillionSecondToTimeTag(t));

        return result + " " + lyric.Trim();
    }

    /// <summary>
    /// Convert the 60000 to [1:00.00]
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

        return $"[{minutes:D2}:{seconds:D2}.{hundredths:D2}]";
    }
}
