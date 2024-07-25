// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser.Kar.Metadata;

public struct KarLyric : IEquatable<KarLyric>
{
    public KarLyric()
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

    public bool Equals(KarLyric other)
    {
        return Text == other.Text && TimeTags.SequenceEqual(other.TimeTags);
    }
}
