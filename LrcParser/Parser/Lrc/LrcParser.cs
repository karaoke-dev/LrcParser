// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Parser.Lrc.Lines;
using LrcParser.Parser.Lrc.Metadata;

namespace LrcParser.Parser.Lrc;

/// <summary>
/// Parser for encode and decode .lrc lyric format
/// </summary>
public class LrcParser : LyricParser, IHasParserConfig<LrcEncodeConfig, LrcDecodeConfig>
{
    public LrcEncodeConfig EncodeConfig { get; set; } = new();
    public LrcDecodeConfig DecodeConfig { get; set; } = new();

    public LrcParser()
    {
        Register<LrcLyricParser>();
    }

    protected override Song PostProcess(List<object> values)
    {
        var lyrics = values.OfType<LrcLyric>();

        return new Song
        {
            Lyrics = lyrics.SelectMany(convertLyric).ToList(),
        };

        static IEnumerable<Lyric> convertLyric(LrcLyric lrcLyric)
        {
            foreach (var startTime in lrcLyric.StartTimes)
            {
                yield return new Lyric
                {
                    Text = lrcLyric.Text,
                    StartTime = startTime,
                    TimeTags = getTimeTags(lrcLyric.TimeTags),
                };
            }
        }

        static SortedDictionary<TextIndex, int?> getTimeTags(SortedDictionary<TextIndex, int> timeTags)
            => new(timeTags.ToDictionary(k => k.Key, v => v.Value as int?));
    }

    protected override IEnumerable<object> PreProcess(Song song)
    {
        var lyrics = song.Lyrics;

        // first, should return the time-tag first.
        // todo: implement the algorithm to combine the lyric with different start time but same time-tag.
        foreach (var lyric in lyrics)
        {
            yield return new LrcLyric
            {
                Text = lyric.Text,
                StartTimes = [lyric.StartTime],
                TimeTags = getTimeTags(lyric.TimeTags),
            };
        }

        // give it a line if contains ruby.
        if (lyrics.Any(l => l.RubyTags.Any()))
        {
            // todo: throw the exception by config.
            // throw new InvalidOperationException("Does not support converting ruby tags to LRC format.");
            yield break;
        }

        yield break;

        static SortedDictionary<TextIndex, int> getTimeTags(SortedDictionary<TextIndex, int?> timeTags)
            => new(timeTags.Where(x => x.Value.HasValue).ToDictionary(k => k.Key, v => v.Value!.Value));
    }
}
