// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Parser.Lrc.Lines;
using LrcParser.Parser.Lrc.Metadata;

namespace LrcParser.Parser.Lrc;

/// <summary>
/// Parser for encode and decode .lrc lyric format
/// </summary>
public class LrcParser : LyricParser
{
    public LrcParser()
    {
        Register<LrcLyricParser>();
    }

    protected override Song PostProcess(List<object> values)
    {
        var lyrics = values.OfType<LrcLyric>();

        return new Song
        {
            Lyrics = lyrics.Select(l => new Lyric
            {
                Text = l.Text,
                TimeTags = getTimeTags(l.TimeTags),
            }).ToList(),
        };

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
        {
            // todo: throw the exception by config.
            // throw new InvalidOperationException("Does not support converting ruby tags to LRC format.");
            yield break;
        }

        yield break;

        static SortedDictionary<TextIndex, int> getTimeTags(SortedDictionary<TextIndex, int?> timeTags, int offsetTime = 0)
            => new(timeTags.Where(x => x.Value.HasValue).ToDictionary(k => k.Key, v => v.Value!.Value + offsetTime));
    }
}
