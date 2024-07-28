// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser.Lrc.Metadata;

public struct LrcLyric : IEquatable<LrcLyric>
{
    public LrcLyric()
    {
    }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Start times for the lyrics.
    /// Because lrc format allows multiple start times for a single line, so it is an array.
    /// </summary>
    public int[] StartTimes { get; set; } = [];

    /// <summary>
    /// Time tags.
    /// It's the absolute time.
    /// </summary>
    public SortedDictionary<TextIndex, int> TimeTags { get; set; } = new();

    public bool Equals(LrcLyric other)
    {
        return Text == other.Text
               && StartTimes.SequenceEqual(other.StartTimes)
               && TimeTags.SequenceEqual(other.TimeTags);
    }
}
