// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Parser.Kar.Utils;

internal static class TimeTagUtils
{
    private const char decimal_point = '.';

    /// <summary>
    /// Convert milliseconds to format [mm:ss.ss].
    /// </summary>
    /// <example>
    /// Input : 17970
    /// Output : [00:17:97]
    /// </example>
    /// <param name="millionSecond"></param>
    /// <returns></returns>
    internal static string MillionSecondToTimeTag(int millionSecond)
    {
        return millionSecond < 0
            ? ""
            : string.Format("[{0:D2}:{1:D2}" + decimal_point + "{2:D2}]", millionSecond / 1000 / 60, millionSecond / 1000 % 60, millionSecond / 10 % 100);
    }

    /// <summary>
    /// Convert format [mm:ss.ss] to milliseconds.
    /// </summary>
    /// <example>
    /// Input : [00:17:97]
    /// Output : 17970
    /// </example>
    /// <param name="timeTag"></param>
    /// <returns></returns>
    internal static int TimeTagToMillionSecond(string timeTag)
    {
        if (timeTag.Length < 10 || timeTag[0] != '[' || !char.IsDigit(timeTag[1]))
            return -1;

        int minute = int.Parse(timeTag.Substring(1, 2));
        int second = int.Parse(timeTag.Substring(4, 2));
        int millionSecond = int.Parse(timeTag.Substring(7, 2));

        return (minute * 60 + second) * 1000 + millionSecond * 10;
    }
}
