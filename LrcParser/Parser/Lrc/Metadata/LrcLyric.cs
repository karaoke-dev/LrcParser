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
    /// Time tags
    /// </summary>
    public SortedDictionary<TextIndex, int> TimeTags { get; set; } = new();

    public bool Equals(LrcLyric other)
    {
        return Text == other.Text && TimeTags.SequenceEqual(other.TimeTags);
    }
}
