// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using LrcParser.Extension;
using LrcParser.Parser.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Parser.Lrc.Utils;

namespace LrcParser.Parser.Lrc.Lines;

public class LrcRubyParser : SingleLineParser<LrcRuby>
{
    public override bool CanDecode(string text)
        => !string.IsNullOrEmpty(text) && text.ToLower().StartsWith("@ruby", StringComparison.Ordinal);

    public override LrcRuby Decode(string text)
    {
        var rubyTextRegex = new Regex("@(Ruby|ruby)(?<index>[0-9]+)=(?<text>.*$)");

        var rubyTagResult = text.Split(',');
        var rubyTextResult = rubyTextRegex.Match(rubyTagResult[0]);

        var parent = rubyTextResult.GetGroupValue<string>("text")!;
        var (ruby, timeTags) = LrcTimedTextUtils.TimedTextToObject(rubyTagResult[1]);
        var startTime = string.IsNullOrEmpty(rubyTagResult.ElementAtOrDefault(2)) ? default(int?) : TimeTagUtils.TimeTagToMillionSecond(rubyTagResult[2]);
        var endTime = string.IsNullOrEmpty(rubyTagResult.ElementAtOrDefault(3)) ? default(int?) : TimeTagUtils.TimeTagToMillionSecond(rubyTagResult[3]);

        return new LrcRuby
        {
            Parent = parent,
            Ruby = ruby,
            TimeTags = timeTags,
            StartTime = startTime,
            EndTime = endTime,
        };
    }

    public override string Encode(LrcRuby component, int index)
    {
        var parent = component.Parent;
        var ruby = LrcTimedTextUtils.ToTimedText(component.Ruby, component.TimeTags);
        var startTime = component.StartTime == null ? "" : TimeTagUtils.MillionSecondToTimeTag(component.StartTime.Value);
        var endTime = component.EndTime == null ? "" : TimeTagUtils.MillionSecondToTimeTag(component.EndTime.Value);

        var input = $"@Ruby{index + 1}={parent},{ruby},{startTime},{endTime}";

        const string remove_last_comma_pattern = "([,]*)$";
        return Regex.Replace(input, remove_last_comma_pattern, "");
    }
}
