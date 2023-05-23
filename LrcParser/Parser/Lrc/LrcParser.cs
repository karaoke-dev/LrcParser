// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using LrcParser.Extension;
using LrcParser.Model;
using LrcParser.Parser.Lrc.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Utils;

namespace LrcParser.Parser.Lrc;

/// <summary>
/// Parser for encode and decode .lrc lyric format
/// </summary>
public class LrcParser : LyricParser
{
    public LrcParser()
    {
        Register<LrcRubyParser>();
        Register<LrcLyricParser>();
    }

    protected override Song PostProcess(List<object> values)
    {
        var lyrics = values.OfType<LrcLyric>();
        var rubies = values.OfType<LrcRuby>();

        return new Song
        {
            Lyrics = lyrics.Select(l => new Lyric
            {
                Text = l.Text,
                TimeTags = getTimeTags(l.TimeTags),
                RubyTags = getRubyTags(rubies, l).ToList()
            }).ToList()
        };

        static IEnumerable<RubyTag> getRubyTags(IEnumerable<LrcRuby> rubyTags, LrcLyric lyric)
        {
            var text = lyric.Text;
            var timeTags = lyric.TimeTags;

            foreach (var rubyTag in rubyTags)
            {
                if (string.IsNullOrEmpty(rubyTag.Ruby) || string.IsNullOrEmpty(rubyTag.Parent))
                    continue;

                if (rubyTag.Ruby == rubyTag.Parent)
                    continue;

                var hasStartTime = rubyTag.StartTime.HasValue;
                var hasEndTime = rubyTag.EndTime.HasValue;

                var matches = new Regex(rubyTag.Parent).Matches(text);

                foreach (var match in matches.ToArray())
                {
                    var startTextIndex = match.Index;
                    var endTextIndex = startTextIndex + match.Length;

                    var startTimeTag = timeTags.Reverse().LastOrDefault(x => TextIndexUtils.ToGapIndex(x.Key) >= startTextIndex);
                    var endTimeTag = timeTags.FirstOrDefault(x => TextIndexUtils.ToGapIndex(x.Key) >= endTextIndex);

                    if (!hasStartTime && !hasEndTime)
                    {
                        yield return new RubyTag
                        {
                            Text = rubyTag.Ruby,
                            TimeTags = getTimeTags(rubyTag.TimeTags, startTimeTag.Value),
                            StartIndex = startTextIndex,
                            EndIndex = endTextIndex
                        };
                    }
                    else
                    {
                        // should not add the ruby if is not in the time-range.
                        if (hasStartTime && rubyTag.StartTime > startTimeTag.Value)
                            continue;

                        if (hasEndTime && rubyTag.EndTime < endTimeTag.Value)
                            continue;

                        yield return new RubyTag
                        {
                            Text = rubyTag.Ruby,
                            TimeTags = getTimeTags(rubyTag.TimeTags, startTimeTag.Value),
                            StartIndex = TextIndexUtils.ToGapIndex(startTimeTag.Key),
                            EndIndex = TextIndexUtils.ToGapIndex(endTimeTag.Key)
                        };
                    }
                }
            }
        }

        static SortedDictionary<TextIndex, int?> getTimeTags(SortedDictionary<TextIndex, int> timeTags, int offsetTime = 0)
            => new(timeTags.ToDictionary(k => k.Key, v => v.Value + offsetTime as int?));
    }

    protected override IEnumerable<object> PreProcess(Song song)
    {
        var lyrics = song.Lyrics;

        // first, should return the time-tag first.
        foreach (var lyric in lyrics)
        {
            yield return new LrcLyric
            {
                Text = lyric.Text,
                TimeTags = getTimeTags(lyric.TimeTags),
            };
        }

        // give it a line if contains ruby.
        if (lyrics.Any(l => l.RubyTags.Any()))
            yield return new object();

        // then, export the ruby.
        // should group by parent first because merge the ruby should not be affect by those rubies with different ruby.
        var rubiesWithSameParent = lyrics.Select(getRubyTags).SelectMany(x => x).GroupBy(x => x.Parent);

        foreach (var groupWithSameParent in rubiesWithSameParent)
        {
            // should group with continuous ruby.
            var rubiesWithSameRuby = groupWithSameParent
                                     .OrderBy(x => x.StartTime).ThenBy(x => x.EndTime)
                                     .GroupByContinuous(x => new RubyGroup
                                     {
                                         Ruby = x.Ruby,
                                         TimeTags = x.TimeTags
                                     }).ToList();

            // then, combine those continuous ruby.
            foreach (var groupWithSameRuby in rubiesWithSameRuby)
            {
                var ruby = groupWithSameRuby.Key.Ruby;
                var parent = groupWithSameParent.Key;
                var timeTags = groupWithSameRuby.Key.TimeTags;

                // should process the value with same parent text and ruby text.
                var isFirst = rubiesWithSameRuby.IndexOf(groupWithSameRuby) == 0;
                var isLast = rubiesWithSameRuby.IndexOf(groupWithSameRuby) == rubiesWithSameRuby.Count - 1;

                var minStartTime = isFirst ? null : groupWithSameRuby.Min(x => x.StartTime);
                var maxEndTime = isLast ? null : groupWithSameRuby.Max(x => x.EndTime);

                yield return new LrcRuby
                {
                    Ruby = ruby,
                    Parent = parent,
                    TimeTags = timeTags,
                    StartTime = minStartTime,
                    EndTime = maxEndTime
                };
            }
        }

        static IEnumerable<LrcRuby> getRubyTags(Lyric lyric)
        {
            var timeTags = lyric.TimeTags;

            foreach (var rubyTag in lyric.RubyTags)
            {
                var startIndex = rubyTag.StartIndex;
                var endIndex = rubyTag.EndIndex;

                var startTimeTag = timeTags.Reverse().LastOrDefault(x => TextIndexUtils.ToGapIndex(x.Key) >= startIndex && x.Value.HasValue).Value;
                var endTimeTag = timeTags.FirstOrDefault(x => TextIndexUtils.ToGapIndex(x.Key) >= endIndex && x.Value.HasValue).Value;

                yield return new LrcRuby
                {
                    Ruby = rubyTag.Text,
                    Parent = lyric.Text[startIndex..endIndex],
                    TimeTags = getTimeTags(rubyTag.TimeTags, -startTimeTag ?? 0),
                    StartTime = startTimeTag,
                    EndTime = endTimeTag,
                };
            }
        }

        static SortedDictionary<TextIndex, int> getTimeTags(SortedDictionary<TextIndex, int?> timeTags, int offsetTime = 0)
            => new(timeTags.Where(x => x.Value.HasValue).ToDictionary(k => k.Key, v => v.Value!.Value + offsetTime));
    }

    private struct RubyGroup : IEquatable<RubyGroup>
    {
        public string Ruby { get; set; }

        public SortedDictionary<TextIndex, int> TimeTags { get; set; }

        public bool Equals(RubyGroup other)
        {
            return Ruby == other.Ruby && TimeTags.SequenceEqual(other.TimeTags);
        }

        public override bool Equals(object? obj)
        {
            return obj is RubyGroup other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ruby);
        }
    }
}
